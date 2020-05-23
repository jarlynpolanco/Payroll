using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Payroll.Shared.Models;
using System.IO;
using System.Text;
using Microsoft.Extensions.Options;
using Payroll.Shared.Statics;
using Payroll.Shared.Exceptions;
using System.Net;

namespace Payroll.Services
{
    public class FileManagementService
    {
        private readonly EmployeeService _employeeService;
        private readonly SftpManagementService _sftpManagementService;
        private readonly PgpEncryptionService _pgpEncryptionService;

        public FileManagementService(EmployeeService employeeService, SftpManagementService sftpManagementService, 
            PgpEncryptionService pgpEncryptionService, IOptions<AppSettings> appSettings)
        {
            _employeeService = employeeService;
            _sftpManagementService = sftpManagementService;
            _pgpEncryptionService = pgpEncryptionService;
            ConnectionStrings.MySqlConnectionString = appSettings.Value.MySqlConnectionString;
        }

        public string GenerateOutPutFile(DateTime payrollDate) 
        {
            var employees = _employeeService.GetAll("Payrolls");

            if(employees == null || employees.Count() == 0)
                throw new HttpStatusException($"No hay empleados disponibles. Favor revisar la fuente de datos.",
                    HttpStatusCode.Forbidden);

            var totalAmount = employees?.Sum(x => x.Payrolls
                ?.FirstOrDefault(p => p.PayrollDate.Date == payrollDate.Date)?.NetSalary);

            if(totalAmount == null || totalAmount < 1)
                throw new HttpStatusException($"No hay nominas disponibles para la fecha indicada {payrollDate:dd/MM/yyyy}",
                    HttpStatusCode.Forbidden);

            var payrollHeader = new List<EmployeePayrollHeader>()
            {
                new EmployeePayrollHeader()
                {
                    CompanyRnc = "401005107",
                    PayrollPaymentDay = $"{payrollDate.Date:dd/MM/yyyy}",
                    RowsQuantity = employees.Count().ToString(),
                    SourceAccountNumber = "11112681455",
                    TotalAmount = totalAmount.ToString(),
                    TransmissionDate = $"{DateTime.Now:dd/MM/yyyy}"
                } 
            };

            var engineHeader = new FileHelperEngine<EmployeePayrollHeader>();

            var engine = new FileHelperEngine<EmployeePayrollDetail>()
            {
                Encoding = new UTF8Encoding(false),
                HeaderText = engineHeader.WriteString(payrollHeader)
            };
            
            int rowsNumber = employees.Count();
            List<EmployeePayrollDetail> payrollList = new List<EmployeePayrollDetail>();

            employees.ToList().ForEach(item =>
            {
                var payroll = item.Payrolls.FirstOrDefault(x => x.PayrollDate.Date == payrollDate.Date);
                var employeePayroll = new EmployeePayrollDetail()
                {
                    NetIncome = payroll.NetSalary.ToString(),
                    AccountNumber = item.AccountNumber,
                    DocumentNumber = item.DocumentNumber
                };
                payrollList.Add(employeePayroll);
            });

            string fileName = $"{Guid.NewGuid().ToString().Replace("-", string.Empty)}.gpg";

            using (var stream = new MemoryStream())
            using (var streamWriter = new StreamWriter(stream))
            {
                engine.WriteStream(streamWriter, payrollList);
                streamWriter.AutoFlush = true;
                stream.Position = 0;

                using var streamReader = new StreamReader(stream);
                var encryptedFile = _pgpEncryptionService.EncryptStreamFile(streamReader);
                _sftpManagementService.SftpUploadFile(encryptedFile, fileName);
            }
            return fileName;
        }

        public EmployeePayroll GetOutPutFile(string fileName) 
        {
            var engineDetail = new FileHelperEngine<EmployeePayrollDetail>();
            var engineHeader = new FileHelperEngine<EmployeePayrollHeader>();

            var fullFileName = _sftpManagementService.SftpDownloadFile(fileName);

            if(!File.Exists(fullFileName))
                throw new HttpStatusException($"El archivo con el nombre indicado: {fileName} no existe en el SFTP.",
                    HttpStatusCode.Forbidden);

            var streamFile = _pgpEncryptionService.DescryptFileAsStream(fullFileName);
            using TextReader textReader = new StreamReader(streamFile);
            var employeePayroll = new EmployeePayroll()
            {
                EmployeePayrollDetail = engineDetail.ReadStream(textReader),
                EmployeePayrollHeader = engineHeader.ReadString(engineDetail.HeaderText).FirstOrDefault()
            };

            return employeePayroll;
        }
    }
}
