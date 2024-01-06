using Microsoft.AspNetCore.Http;

namespace Payslip.Application.Commands
{
    public class ImportUserExcelCommmand
    {
        public IFormFile File { get; set; }
    }
}
