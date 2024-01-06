using Microsoft.IdentityModel.Tokens;

namespace Payslip.Application.Base
{
    public class JwTToken
    {
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public int expires_in { get; set; }
    }

    public class JwtIssuerOptionsModel
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTime Expiration => IssuedAt.AddMinutes(ValidTimeInMinute);
        public DateTime MobileExpiration => IssuedAt.AddMinutes(ExpireTimeTokenInMinute);
        public DateTime NotBefore => DateTime.Now;
        public DateTime IssuedAt => DateTime.Now;
        public int ValidTimeInMinute { get; set; }
        public int ExpireTimeTokenInMinute { get; set; }
        public string SecretKey { get; set; }
        public Func<Task<string>> JtiGenerator =>
            () => Task.FromResult(Guid.NewGuid().ToString());
        public SigningCredentials SigningCredentials { get; set; }
    }

    public class ConnectionStringsModel
    {
        public string Main { get; set; }
    }
}
