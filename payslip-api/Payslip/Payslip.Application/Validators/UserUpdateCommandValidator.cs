using FluentValidation;
using Payslip.Application.Commands;

namespace Payslip.Application.Validators
{
    public class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
    {
        public UserUpdateCommandValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().WithMessage("نام الزامی است.");
            RuleFor(c => c.LastName).NotEmpty().WithMessage("نام خانوادگی الزامی است.");
            RuleFor(c => c.NationalCode).NotEmpty().WithMessage("کد ملی الزامی است.");
            RuleFor(c => c.CardNumber).NotEmpty().WithMessage("شماره کارت الزامی است.");
            RuleFor(c => c.Password).NotEmpty().WithMessage("رمز عبور جدید اجباری است.");
            RuleFor(c => c.Password).Must(c => !c.Contains(" ")).WithMessage("رمز عبور نمی‌تواند شامل کاراکتر فاصله باشد.");
            RuleFor(c => c.Password).Must(c => c.Trim().Length >= 8).WithMessage("رمز عبور باید حداقل شامل 8 کاراکتر باشد.");

            RuleFor(c => c.NationalCode).Must(c => ulong.TryParse(c, out _)).When(c => !string.IsNullOrEmpty(c.NationalCode)).WithMessage("فرمت کد ملی صحیح نیست.");
            RuleFor(c => c.CardNumber).Must(c => ulong.TryParse(c, out _)).When(c => !string.IsNullOrEmpty(c.CardNumber)).WithMessage("فرمت شماره کارت صحیح نیست.");
        }
    }
}