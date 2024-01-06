using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
            .AddJsonFile("appsettings.json")
            .Build();

            AppSettingsModel appSettingsModel = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CONFIG")) ? config.Get<AppSettingsModel>() :
                JsonConvert.DeserializeObject<AppSettingsModel>(Environment.GetEnvironmentVariable("CONFIG")!)!;

            _jwtIssuerOptions = appSettingsModel.JwtIssuerOptions;

        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            command.Validate();

            await _authenticationService.ChangePassword(UserId, command);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("ChangePasswordByAdmin")]
        public async Task<IActionResult> ChangePasswordByAdmin(ChangePasswordByAdminCommand command)
        {
            command.Validate();

            await _authenticationService.ChangePasswordByAdmin(command.UserId, command);

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
