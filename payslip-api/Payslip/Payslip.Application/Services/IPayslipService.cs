using Payslip.Application.Commands;

namespace Payslip.Application.Services
{
    public interface IPayslipService
    {
        Task CreatePayslips(IEnumerable<PayslipCommand> payslips);
    }
}
