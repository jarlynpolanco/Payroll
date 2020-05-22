using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Payroll.Shared.Models;
using System.IO;

namespace Payroll.Services
{
    public class FileManagementService
    {
        private readonly EmployeeService _employeeService;
        private readonly SftpManagementService _sftpManagementService;

        public FileManagementService(EmployeeService employeeService,
            SftpManagementService sftpManagementService)
        {
            _employeeService = employeeService;
            _sftpManagementService = sftpManagementService;
        }

        public string GenerateOutPutFile(DateTime payrollDate) 
        {
            var engine = new FileHelperEngine<EmployeePayroll>();

            var employees = _employeeService.GetAll("Payrolls");
            int rowsNumber = employees.Count();
            List<EmployeePayroll> payrollList = new List<EmployeePayroll>();

            foreach(var item in employees) 
            {
                var payroll = item.Payrolls.FirstOrDefault(x => x.PayrollDate.Date == payrollDate.Date);
                var employeePayroll = new EmployeePayroll()
                {
                    AccountNumberSourceEntity = "11112681455",
                    DocumentNumberEmployee = item.DocumentNumber,
                    NetIncome = payroll.NetSalary.ToString(),
                    RecordsNumber = rowsNumber.ToString(),
                    RncSourceEntity = "401005107"
                };
                payrollList.Add(employeePayroll);
            }
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

        public IList<EmployeePayroll> GetOutPutFile(string fileName) 
        {
            var fullFileName = _sftpManagementService.SftpDownloadFile(fileName);

            return new FileHelperEngine<EmployeePayroll>().ReadFileAsList(fullFileName);
        }
    }
}
