using Payslip.Application.Commands;
using Payslip.Application.DTOs;
using Payslip.Core.Entities;

namespace Payslip.Application.Services
{
    public interface IPayslipService
    {
        Task CreatePayslips(IEnumerable<PayslipCommand> payslips, int year, int month, Guid fileId);
        Task<IEnumerable<UserPayslipWagesDTO>> GetUserWages(Guid userId);
    }
}
