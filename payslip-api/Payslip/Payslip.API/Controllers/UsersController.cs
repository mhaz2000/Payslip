using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payslip.API.Base;
using Payslip.Application.Services;

namespace Payslip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetUsers(int? skip = 0)
        {
            var data = _userService.GetUsers(skip ?? 0);

            return Ok(new ResponseModel(data.Total, data.Users));
        }
    }
}
