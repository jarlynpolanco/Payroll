using FileHelpers;

namespace Payroll.Shared.Models
{
    [FixedLengthRecord(FixedMode.AllowLessChars)]
    public class EmployeePayrollHeader
    {
        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both)]
        public string SourceAccountNumber { get; set; }

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both)]
        public string TransmissionDate { get; set; }

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both)]
        public string PayrollPaymentDay { get; set; }

        [FieldFixedLength(20)]
        [FieldTrim(TrimMode.Both)]
        public string TotalAmount { get; set; }

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both)]
        public string CompanyRnc { get; set; }

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both)]
        public string RowsQuantity { get; set; }
    }
}
