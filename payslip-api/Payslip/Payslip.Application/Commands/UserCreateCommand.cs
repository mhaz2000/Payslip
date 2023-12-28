using Payslip.Application.Validators;
using Payslip.Application.Validators.Extensions;

namespace Payslip.Application.Commands
{
    public class UserCreateCommand : ICommandBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string CardNumber { get; set; }

        public void Validate() => new UserCreateCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
