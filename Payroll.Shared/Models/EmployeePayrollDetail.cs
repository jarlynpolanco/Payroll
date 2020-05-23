using FileHelpers;

namespace Payroll.Shared.Models
{
    [FixedLengthRecord(FixedMode.AllowLessChars)]
    [IgnoreFirst()]
    public class EmployeePayrollDetail
    {
        [FieldFixedLength(11)]
        [FieldTrim(TrimMode.Both)]
        public string AccountNumber { get; set; }

        [FieldFixedLength(11)]
        [FieldTrim(TrimMode.Both)]
        public string DocumentNumber { get; set; }

        [FieldFixedLength(7)]
        [FieldTrim(TrimMode.Both)]
        public string NetIncome { get; set; }
    }
}
