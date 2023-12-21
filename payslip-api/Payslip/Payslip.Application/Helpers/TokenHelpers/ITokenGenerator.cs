using Microsoft.AspNetCore.Identity;
using Payslip.Application.Base;
using Payslip.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payslip.Application.Helpers.TokenHelpers
{
    public interface ITokenGenerator
    {
        JwTToken TokenGeneration(User user, JwtIssuerOptionsModel jwtOptions, IList<IdentityRole<Guid>> userRoles);
    }
}
