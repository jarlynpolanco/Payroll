using System;
using Microsoft.AspNetCore.Mvc;
using Payroll.Services;
using Payroll.Shared.Models;

namespace Payroll.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly FileManagementService _fileManagementService;

        public PayrollController(FileManagementService fileManagementService) =>
             _fileManagementService = fileManagementService;

        [HttpGet("{payrollDate}")]
        public ActionResult<GenericResponse<string>> GeneratePayrollFile(DateTime payrollDate)
        {
            return Ok(new GenericResponse<string>()
            {
                Data = _fileManagementService.GenerateOutPutFile(payrollDate),
                Success = true
            });
        }

        [HttpGet("{fileName}")]
        public ActionResult<GenericResponse<EmployeePayroll>> ReadPayrollFile(string fileName)
        {
            return Ok(new GenericResponse<EmployeePayroll>()
            {
                Data = _fileManagementService.GetOutPutFile(fileName),
                Success = true
            });
        }
    }
}
