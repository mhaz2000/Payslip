using System.IdentityModel.Tokens.Jwt;

namespace Payslip.Application.Helpers.TokenHelpers
{
    public class JwtTokenHelper : IJwtTokenHelper
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        public JwtTokenHelper()
        {
            if (_jwtSecurityTokenHandler == null)
                _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public string WriteToken(JwtSecurityToken jwt)
        {
            return _jwtSecurityTokenHandler.WriteToken(jwt);
        }
    }
}
