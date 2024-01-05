using FluentValidation;
using Payslip.Application.Commands;

namespace Payslip.Application.Validators
{
    public class ChangePasswordByAdminCommandValidator : AbstractValidator<ChangePasswordByAdminCommand>
    {
        public ChangePasswordByAdminCommandValidator()
        {
            RuleFor(c => c.UserId).NotNull().WithMessage("شناسه کاربر الزامی است.");

            RuleFor(c => c.NewPassword).NotEmpty().WithMessage("رمز عبور جدید اجباری است.");
            RuleFor(c => c.NewPassword).Must(c => !c.Contains(" ")).When(c => !string.IsNullOrEmpty(c.NewPassword)).WithMessage("رمز عبور نمی‌تواند شامل کاراکتر فاصله باشد.");
            RuleFor(c => c.NewPassword).Must(c => c.Trim().Length >= 8).When(c => !string.IsNullOrEmpty(c.NewPassword)).WithMessage("رمز عبور باید حداقل شامل 8 کاراکتر باشد.");
        }
    }
}
