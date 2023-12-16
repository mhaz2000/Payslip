using Payslip.Application.Commands;

namespace Payslip.API.Helpers
{
    public interface IPayslipExtractorHelpler
    {
        IEnumerable<PayslipCommand> Extract(Stream stream);
    }
}
