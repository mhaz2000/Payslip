using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payslip.Application.Helpers.TokenHelpers
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}
