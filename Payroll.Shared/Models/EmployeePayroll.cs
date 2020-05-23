using System.Collections.Generic;

namespace Payroll.Shared.Models
{
    public class EmployeePayroll
    {
        public EmployeePayrollHeader EmployeePayrollHeader { get; set; }
        public IList<EmployeePayrollDetail> EmployeePayrollDetail { get; set; }
    }
}
