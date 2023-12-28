using FluentValidation;
using Payslip.Application.Commands;

namespace Payslip.Application.Validators
{
    public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
    {
        public UserCreateCommandValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().WithMessage("نام الزامی است.");
            RuleFor(c => c.LastName).NotEmpty().WithMessage("نام خانوادگی الزامی است.");
            RuleFor(c => c.NationalCode).NotEmpty().WithMessage("کد ملی الزامی است.");
            RuleFor(c => c.CardNumber).NotEmpty().WithMessage("شماره کارت الزامی است.");

            RuleFor(c => c.NationalCode).Must(c => ulong.TryParse(c, out _)).When(c=> !string.IsNullOrEmpty(c.NationalCode)).WithMessage("فرمت کد ملی صحیح نیست.");
            RuleFor(c => c.CardNumber).Must(c => ulong.TryParse(c, out _)).When(c => !string.IsNullOrEmpty(c.CardNumber)).WithMessage("فرمت شماره کارت صحیح نیست.");
        }
    }
}