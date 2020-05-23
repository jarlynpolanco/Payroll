using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Payroll.Shared.Models;
using System.IO;
using System.Text;
using Microsoft.Extensions.Options;
using Payroll.Shared.Statics;

namespace Payroll.Services
{
    public class FileManagementService
    {
        private readonly EmployeeService _employeeService;
        private readonly SftpManagementService _sftpManagementService;

        public FileManagementService(EmployeeService employeeService, SftpManagementService sftpManagementService, 
            IOptions<AppSettings> appSettings)
        {
            _employeeService = employeeService;
            _sftpManagementService = sftpManagementService;
            ConnectionStrings.MySqlConnectionString = appSettings.Value.MySqlConnectionString;
        }

        public string GenerateOutPutFile(DateTime payrollDate) 
        {
            var employees = _employeeService.GetAll("Payrolls");
            var totalAmount = employees.Sum(x => x.Payrolls
                .FirstOrDefault(p => p.PayrollDate.Date == payrollDate.Date).NetSalary);

            var payrollHeader = new List<EmployeePayrollHeader>()
            {
                new EmployeePayrollHeader()
                {
                    CompanyRnc = "401005107",
                    PayrollPaymentDay = payrollDate.Date.ToString("dd/MM/yyyy"),
                    RowsQuantity = employees.Count().ToString(),
                    SourceAccountNumber = "11112681455",
                    TotalAmount = totalAmount.ToString(),
                    TransmissionDate = DateTime.Now.ToString("dd/MM/yyyy")
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

            string fileName = $"{Guid.NewGuid().ToString().Replace("-", string.Empty)}.txt";

            using (var stream = new MemoryStream())
            using (var streamWriter = new StreamWriter(stream))
            {
                engine.WriteStream(streamWriter, payrollList);
                streamWriter.AutoFlush = true;
                stream.Position = 0;

                _sftpManagementService.SftpUploadFile(stream, fileName);
            }

            return fileName;
        }

        public EmployeePayroll GetOutPutFile(string fileName) 
        {
            var fullFileName = _sftpManagementService.SftpDownloadFile(fileName);
            var engineDetail = new FileHelperEngine<EmployeePayrollDetail>();
            var engineHeader = new FileHelperEngine<EmployeePayrollHeader>();

            var employeePayroll = new EmployeePayroll()
            {
                EmployeePayrollDetail = engineDetail.ReadFileAsList(fullFileName),
                EmployeePayrollHeader = engineHeader.ReadString(engineDetail.HeaderText).FirstOrDefault()
            };

            return employeePayroll;
        }
    }
}
