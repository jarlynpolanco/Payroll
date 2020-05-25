using System;
using System.Net;

namespace Payroll.Shared.Exceptions
{
    public class HttpStatusException : Exception
    {
        public HttpStatusCode? StatusCode { get; set; }

        public HttpStatusException(string message, HttpStatusCode statusCode) : base(message) =>
            this.StatusCode = statusCode;
    }
}
