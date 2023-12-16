using System.IdentityModel.Tokens.Jwt;

namespace Payslip.Application.Helpers
{
    public interface IJwtTokenHelper
    {
        string WriteToken(JwtSecurityToken jwt);
    }
}
