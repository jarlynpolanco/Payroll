using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Shared.Models
{
    public class GenericResponse<T>
    {
        public GenericResponse() { }
        public bool Success { get; set; } = false;
        public T Data { get; set; }
    }
}