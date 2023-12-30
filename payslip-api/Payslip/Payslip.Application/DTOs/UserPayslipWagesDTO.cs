using Payslip.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payslip.Application.DTOs
{
    public class UserPayslipWagesDTO
    {
        public int Year { get; set; }
        public IEnumerable<int> Months { get; set; }
    }
}
