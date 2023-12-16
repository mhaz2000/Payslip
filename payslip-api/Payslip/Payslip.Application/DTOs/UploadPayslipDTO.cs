using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payslip.Application.DTOs
{
    public class UploadPayslipCommand
    {
        public IFormFile File { get; set; }
    }
}
