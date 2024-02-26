using Payslip.Application.Commands;
using Payslip.Application.DTOs;
using Payslip.Core.Entities;

namespace Payslip.Application.Services
{
    public interface IPayslipService
    {
        Task CreatePayslips(IEnumerable<PayslipCommand> payslips, int year, int month, Guid fileId);
        Task<(IEnumerable<PayslipDTO> Payslips, int Total)> GetPayslips(int skip);
        Task<UserPayslipDTO> GetUserPayslip(Guid userId, int month, int year);
        Task<IEnumerable<UserPayslipWagesDTO>> GetUserWages(Guid userId);
        Task RemovePayslip(Guid id);
    }
}
