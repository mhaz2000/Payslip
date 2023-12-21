using Microsoft.AspNetCore.Identity;
using Payslip.Application.Base;
using Payslip.Core.Entities;
using System.Security.Claims;

namespace Payslip.Application.Helpers.TokenHelpers
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;

        public TokenGenerator(IJwtFactory jwtFactory, ITokenFactory tokenFactory)
        {
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
        }

        public JwTToken TokenGeneration(User user, JwtIssuerOptionsModel jwtOptions, IList<IdentityRole<Guid>> userRoles)
        {
            var refreshToken = _tokenFactory.GenerateToken();

            var identity = _jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id.ToString());
            if (identity == null)
                throw new SystemException("در فراخوانی و تطابق اطلاعات حساب کاربری خطایی رخ داده است!");

            var userRoleNames = userRoles != null ? userRoles.Select(c => c.Name).ToList() : null;
            var userRoleIds = userRoles != null ? userRoles.Select(c => c.Id.ToString()).ToList() : null;

            var generatedToken = GenerateJwt(user, userRoleNames!, userRoleIds!, identity, _jwtFactory,
                refreshToken, jwtOptions.ExpireTimeTokenInMinute);

            return generatedToken;
        }

        public static JwTToken GenerateJwt(User user, IList<string> userRoles, IReadOnlyCollection<string> userRoleIds, ClaimsIdentity identity,
            IJwtFactory jwtFactory, string refreshToken, int refreshTime)
        {
            var result = new JwTToken
            {
                AuthToken = jwtFactory.GenerateEncodedToken(user, userRoles, userRoleIds, identity),
                RefreshToken = refreshToken,
                expires_in = refreshTime
            };

            return result;
        }
    }
}
