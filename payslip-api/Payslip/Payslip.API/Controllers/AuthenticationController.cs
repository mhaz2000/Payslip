using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payslip.API.Base;
using Payslip.Application.Base;
using Payslip.Application.Commands;
using Payslip.Application.Services;

namespace Payslip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly JwtIssuerOptionsModel _jwtIssuerOptions;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;

            var config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appSettings.json")
           .Build();

            _jwtIssuerOptions = config.Get<AppSettingsModel>().JwtIssuerOptions;

        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            command.Validate();

            await _authenticationService.ChangePassword(UserId, command);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand loginDto)
        {
            loginDto.Validate();

            var loginResult = await _authenticationService.Login(loginDto, _jwtIssuerOptions);

            return Ok(loginResult);
        }
    }
}
