using Payslip.Application.Validators;
using Payslip.Application.Validators.Extensions;

namespace Payslip.Application.Commands
{
    public class LoginCommand : ICommandBase
    {
        public LoginCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public void Validate() => new LoginCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
