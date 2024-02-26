using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payslip.API.Base;
using Payslip.API.Helpers;
using Payslip.Application.Commands;
using Payslip.Application.Services;
using Payslip.Core.Enums;

namespace Payslip.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IExcelHelpler _userExctractor;

        public UsersController(IUserService userService, IExcelHelpler excelHelpler)
        {
            _userService = userService;
            _userExctractor = excelHelpler;
        }

        public async Task<IActionResult> CreateUser(UserCreateCommand command)
        {
            command.Validate();

            await _userService.CreateUser(command);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(int? skip = 0, string? search = "")
        {
            var data = await _userService.GetUsers(skip ?? 0, search ?? string.Empty);

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

        [HttpPost("UploadUsersFile")]
        public async Task<IActionResult> ImportUsers([FromQuery] ImportUserExcelCommmand dto)
        {
            IEnumerable<UserModel> users;
            using (var stream = new MemoryStream())
            {
                await dto.File.CopyToAsync(stream);
                try
                {
                    users = _userExctractor.ExtractUsers(stream).ToList();
                }
                catch (Exception)
                {
                    return BadRequest("قالب فایل بارگذاری شده صحیح نیست.");
                }
            }

            var command = users.Select(user => new UserCreateCommand()
            {
                CardNumber = user.CardNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NationalCode = user.NationalCode,
            });

            await _userService.CreateUsers(command);

            return Ok();
        }
    }
}
