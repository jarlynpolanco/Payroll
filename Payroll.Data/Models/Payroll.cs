using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Data.Models
{
    [Table("PAYROLL")]
    public class Payroll
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }
        [Column("PAYROLL_DATE")]
        public DateTime PayrollDate { get; set; }
        [Column("ISR_DISCOUNT")]
        public double? ISRDiscount { get; set; }
        [Column("AFP_DISCOUNT")]
        public double? AFPDiscount { get; set; }
        [Column("OTHERS_DISCOUNTS")]
        public double? OthersDiscounts { get; set; }
        [Column("TOTAL_DISCOUNTS")]
        public double? TotalDiscounts { get; set; }
        [Column("NET_SALARY")]
        public double? NetSalary { get; set; }

        [Column("EMPLOYEE_ID")]
        public int? EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
