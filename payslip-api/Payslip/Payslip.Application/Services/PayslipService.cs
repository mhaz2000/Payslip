using AutoMapper;
using Payslip.Application.Base;
using Payslip.Application.Commands;
using Payslip.Application.DTOs;
using Payslip.Application.Helpers;
using Payslip.Core.Entities;
using Payslip.Core.Enums;
using Payslip.Core.Repositories.Base;

namespace Payslip.Application.Services
{
    public class PayslipService : IPayslipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PayslipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreatePayslips(IEnumerable<PayslipCommand> payslipsCommand, int year, int month)
        {
            (year, Month parsedMonth) = DateHelper.ValidateDate(year, month);

            var payslips = _mapper.Map<IEnumerable<UserPayslip>>(payslipsCommand);
            if (!payslipsCommand.Any())
                throw new ManagedException("خطایی در خواندن مقادیر اکسل ارسالی رخ داده است.");

            foreach (var payslip in payslips)
            {
                payslip.Month = parsedMonth;
                payslip.Year = year;
            }

            await _unitOfWork.PayslipRepository.AddRangeAsync(payslips);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<UserPayslipWagesDTO>> GetUserWages(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربر مورد نظر یافت نشد.");

            var payslips = _unitOfWork.PayslipRepository.GetUserPayslips(userId);

            return payslips.GroupBy(c => c.Year).Select(item => new UserPayslipWagesDTO()
            {
                Year = item.Key,
                Months = item.Select(c => c.Month)
            });
        }
    }
}
