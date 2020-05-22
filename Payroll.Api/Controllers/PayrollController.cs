using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Payroll.Services;

namespace Payroll.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly FileManagementService _fileManagementService;

        public PayrollController(FileManagementService fileManagementService) 
        {
            _fileManagementService = fileManagementService;
        }

        [HttpGet("{payrollDate}")]
        public IActionResult GeneratePayrollFile(DateTime payrollDate)
        {
            return Ok(_fileManagementService.GenerateOutPutFile(payrollDate));
        }

        [HttpGet("{fileName}")]
        public IActionResult ReadPayrollFile(string fileName)
        {
            return Ok(_fileManagementService.GetOutPutFile(fileName));
        }
    }
}
