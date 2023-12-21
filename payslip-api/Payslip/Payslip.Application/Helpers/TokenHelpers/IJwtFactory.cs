using Payslip.Core.Entities;
using System.Security.Claims;

namespace Payslip.Application.Helpers.TokenHelpers
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken(User user, IList<string> userRoles, IEnumerable<string> roleIds, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}

