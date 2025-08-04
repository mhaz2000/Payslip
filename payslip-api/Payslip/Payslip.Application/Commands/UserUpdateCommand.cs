using Payslip.Application.Validators;
using Payslip.Application.Validators.Extensions;

namespace Payslip.Application.Commands
{
    public class UserUpdateCommand : ICommandBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string CardNumber { get; set; }
        public string Password { get; set; }

        public void Validate() => new UserUpdateCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
