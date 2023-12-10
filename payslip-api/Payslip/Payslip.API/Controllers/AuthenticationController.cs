using Microsoft.AspNetCore.Mvc;
using Payslip.Application.Commands;
using Payslip.Application.Services;

namespace Payslip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;


        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand loginDto)
        {
            loginDto.Validate();

            var loginResult = await _authenticationService.Login(loginDto);

            return Ok(loginResult);
        }
    }
}
