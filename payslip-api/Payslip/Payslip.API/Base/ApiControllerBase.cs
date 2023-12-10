using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payslip.API.Extensions;
using System.Security.Claims;

namespace Payslip.API.Base
{
    [Authorize]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {

        protected string AccessToken => Request.GetAccessToken();

        protected virtual string UserName => ClaimHelper.GetClaim<string>(this.AccessToken, ClaimTypes.GivenName);
        protected virtual Guid UserId => ClaimHelper.GetClaim<Guid>(this.AccessToken, "id");

        protected ApiControllerBase()
        {
        }

    }
}
