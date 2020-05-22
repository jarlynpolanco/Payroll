using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Payroll.Shared.Models;
using System.IO;
using Microsoft.Extensions.Options;

namespace Payroll.Services
{
    public class FileManagementService
    {
        private readonly EmployeeService _employeeService;
        private readonly IOptions<AppSettings> _appSettings;

        public FileManagementService(EmployeeService employeeService, IOptions<AppSettings> appSettings)
        {
            _employeeService = employeeService;
            _appSettings = appSettings;
        }

        public string GenerateOutPutFile(DateTime payrollDate) 
        {
            var engine = new FileHelperEngine<EmployeePayroll>();

            var employees = _employeeService.GetAll("Payrolls");
            int rowsNumber = employees.Count();
            List<EmployeePayroll> payrollList = new List<EmployeePayroll>();

            foreach(var item in employees) 
            {
                var employeePayroll = item.Payrolls.FirstOrDefault(x => x.PayrollDate.Date == payrollDate.Date);
                var payroll = new EmployeePayroll()
                {
                    AccountNumberSourceEntity = "11112681455",
                    DocumentNumberEmployee = item.DocumentNumber,
                    NetIncome = (employeePayroll.NetSalary / 100).ToString(),
                    RecordsNumber = rowsNumber.ToString(),
                    RncSourceEntity = "401005107"
                };
                payrollList.Add(payroll);
            }
            string fullFileName = Path.Combine(_appSettings.Value.InputFilePath,
                $"{Guid.NewGuid().ToString().Replace("-", string.Empty)}.txt");

            engine.WriteFile(fullFileName, payrollList);

            return Path.GetFileName(fullFileName);
        }

        public IList<EmployeePayroll> GetOutPutFile(string fileName) 
        {
            string fullFileName = Path.Combine(_appSettings.Value.InputFilePath, fileName);

            return new FileHelperEngine<EmployeePayroll>().ReadFileAsList(fullFileName);
        }
    }
}
