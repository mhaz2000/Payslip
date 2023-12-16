using FluentValidation;
using Payslip.Application.Commands;

namespace Payslip.Application.Validators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(c => c.Username).NotNull().WithMessage("نام کاربری الزامی است.");

            RuleFor(c=> c.Password).NotNull().WithMessage("رمز عبور الزامی است.");
        }
    }
}
