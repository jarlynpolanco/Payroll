using FileHelpers;

namespace Payroll.Shared.Models
{
    [DelimitedRecord("|")]
    public class EmployeePayroll
    {
        public string RncSourceEntity { get; set; }
        public string AccountNumberSourceEntity { get; set; }
        public string DocumentNumberEmployee { get; set; }
        public string NetIncome { get; set; }
        public string RecordsNumber { get; set; }
        public string TotalNetIncomes { get; set; }
    }
}
