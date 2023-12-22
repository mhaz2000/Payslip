using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payslip.API.Base;
using Payslip.API.Helpers;
using Payslip.Application.Commands;
using Payslip.Application.DTOs;
using Payslip.Application.Services;
using System.IO;

namespace Payslip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayslipsController : ApiControllerBase
    {
        private readonly IPayslipService _payslipService;
        private readonly IExcelHelpler _payslipExtractorHelpler;
        public PayslipsController(IPayslipService payslipService, IExcelHelpler payslipExtractorHelpler)
        {
            _payslipService = payslipService;
            _payslipExtractorHelpler = payslipExtractorHelpler;
        }

        [HttpGet("wages")]
        public async Task<IActionResult> GetUserPayslipWages()
        {
            var wages = await _payslipService.GetUserWages(UserId);

            return Ok(wages);
        }

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UploadPayslipFile([FromQuery] UploadPayslipCommand dto, int year, int month)
        {
            IEnumerable<PayslipCommand> payslips;
            using (var stream = new MemoryStream())
            {
                await dto.File.CopyToAsync(stream);
                payslips = _payslipExtractorHelpler.ExtractPayslips(stream).ToList();
            }

            await _payslipService.CreatePayslips(payslips, year, month);

            return Ok();
        }
    }
}
