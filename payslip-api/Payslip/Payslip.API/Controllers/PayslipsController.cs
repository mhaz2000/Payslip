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
        private readonly IFileService _fileService;
        private readonly IPayslipService _payslipService;
        private readonly IExcelHelpler _payslipExtractorHelpler;
        public PayslipsController(IPayslipService payslipService, IExcelHelpler payslipExtractorHelpler, IFileService fileService)
        {
            _payslipService = payslipService;
            _payslipExtractorHelpler = payslipExtractorHelpler;
            _fileService = fileService;
        }

        [HttpGet("wages")]
        public async Task<IActionResult> GetUserPayslipWages()
        {
            var wages = await _payslipService.GetUserWages(UserId);

            return Ok(wages);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UploadPayslipFile([FromQuery] UploadPayslipCommand dto, int year, int month)
        {
            IEnumerable<PayslipCommand> payslips;
            Guid fileId;
            using (var stream = new MemoryStream())
            {
                await dto.File.CopyToAsync(stream);
                try
                {
                    payslips = _payslipExtractorHelpler.ExtractPayslips(stream).ToList();
                }
                catch (Exception)
                {
                    return BadRequest("قالب فایل بارگذاری شده صحیح نیست.");
                }
                fileId = await _fileService.StoreFile(stream, dto.File.Name);
            }

            await _payslipService.CreatePayslips(payslips, year, month, fileId);

            return Ok();
        }
    }
}
