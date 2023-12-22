using FluentValidation;
using Payslip.Application.Commands;

namespace Payslip.Application.Validators
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(c=> c.OldPassword).NotEmpty().WithMessage("رمز عبور قبلی اجباری است.");
            RuleFor(c=> c.NewPassword).NotEmpty().WithMessage("رمز عبور جدید اجباری است.");
            RuleFor(c=> c.NewPassword).Must(c=> !c.Contains(" ")).WithMessage("رمز عبور نمی‌تواند شامل کاراکتر فاصله باشد.");
            RuleFor(c=> c.NewPassword).Must(c=> c.Trim().Length>= 8).WithMessage("رمز عبور باید حداقل شامل 8 کاراکتر باشد.");
        }
    }
}
