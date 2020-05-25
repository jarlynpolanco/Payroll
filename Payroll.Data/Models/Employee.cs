using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Data.Models
{
    [Table("EMPLOYEE")]
    public class Employee
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }
        [Column("DOCUMENT_NUMBER")]
        public string DocumentNumber { get; set; }
        [Column("FIRST_NAME")]
        public string FirstName { get; set; }
        [Column("LAST_NAME")]
        public string LastName { get; set; }
        [Column("CRUDE_SALARY")]
        public double? CrudeSalary { get; set; }
        [Column("ACCOUNT_NUMBER")]
        public string AccountNumber { get; set; }

        public virtual IList<PayrollSheet> PayrollsSheet { get; set; }
    }
}
