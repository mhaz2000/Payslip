using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payslip.API.Base;
using Payslip.Application.Commands;
using Payslip.Application.Services;

namespace Payslip.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> CreateUser(UserCreateCommand command)
        {
            command.Validate();

            await _userService.CreateUser(command);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetUsers(int? skip = 0, string? search = "")
        {
            var data = _userService.GetUsers(skip ?? 0, search ?? string.Empty);

            return Ok(new ResponseModel(data.Total, data.Users));
        }

        [HttpPut("ToggleActivation/{userId}")]
        public async Task<IActionResult> ToggleActivation(Guid userId)
        {
            await _userService.ToggleActivation(userId);

            return Ok();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delelte(Guid userId)
        {
            await _userService.RemoveUser(userId);

            return Ok();
        }
    }
}
