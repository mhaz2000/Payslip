using Payslip.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payslip.Application.Services
{
    public class PayslipService : IPayslipService
    {
        public Task CreatePayslips(IEnumerable<PayslipCommand> payslips)
        {
            throw new NotImplementedException();
        }
    }
}
