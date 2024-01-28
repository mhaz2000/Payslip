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
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PayslipService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreatePayslips(IEnumerable<PayslipCommand> payslipsCommand, int year, int month, Guid fileId)
        {
            (year, Month parsedMonth) = DateHelper.ValidateDate(year, month);

            var payslips = _mapper.Map<IEnumerable<UserPayslip>>(payslipsCommand);
            if (!payslipsCommand.Any())
                throw new ManagedException("خطایی در خواندن مقادیر اکسل ارسالی رخ داده است.");

            if (_unitOfWork.PayslipRepository.AsEnumerable().Any(c => c.Month.GetHashCode() == month && c.Year == year))
                throw new ManagedException("فایل فیش حقوقی برای دوره مالی وارد شده، قبلا ثبت شده است.");

            foreach (var payslip in payslips)
            {
                payslip.Month = parsedMonth;
                payslip.Year = year;
                payslip.FileId = fileId;
            }

            await _unitOfWork.PayslipRepository.AddRangeAsync(payslips);
            await _unitOfWork.CommitAsync();
        }

        public (IEnumerable<PayslipDTO> Payslips, int Total) GetPayslips(int skip)
        {
            var payslips = _unitOfWork.PayslipRepository.OrderByDescending(c => c.Year);

            var payslipsDTO = payslips.GroupBy(c => new { c.Year, c.Month, c.FileId }).Select(s => new PayslipDTO()
            {
                FileId = s.Key.FileId,
                Month = s.Key.Month.GetDescription(),
                Year = s.Key.Year,
                UploadedDate = s.FirstOrDefault()!.CreatedAt
            }).ToList().OrderByDescending(c => c.Year).ThenByDescending(c => c.Month);

            return (payslipsDTO.Skip(skip), payslipsDTO.Count());
        }

        public UserPayslipDTO GetUserPayslip(Guid userId, int month, int year)
        {
            var userPayslip = _unitOfWork.PayslipRepository.Include(c => c.User)
                .FirstOrDefault(c => c.User?.Id == userId && c.Month.GetHashCode() == month && c.Year == year);

            return _mapper.Map<UserPayslipDTO>(userPayslip);
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
                Months = item.Select(c => c.Month.GetHashCode())
            });
        }

        public async Task RemovePayslip(Guid id)
        {
            if (!(await _unitOfWork.FileRepository.AnyAsync(c => c.Id == id)))
                throw new ManagedException("فیش مورد نظر یافت نشد.");

            await _fileService.RemoveFile(id);

            var payslips = _unitOfWork.PayslipRepository.Where(c => c.FileId == id);
            _unitOfWork.PayslipRepository.RemoveRange(payslips);

            await _unitOfWork.CommitAsync();
        }
    }
}
