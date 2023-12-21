using System.IdentityModel.Tokens.Jwt;

namespace Payslip.Application.Helpers.TokenHelpers
{
    public interface IJwtTokenHelper
    {
        string WriteToken(JwtSecurityToken jwt);
    }
}
