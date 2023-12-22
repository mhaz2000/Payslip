using Payslip.API.Base;
using Payslip.Application.Commands;

namespace Payslip.API.Helpers
{
    public interface IExcelHelpler
    {
        IEnumerable<PayslipCommand> ExtractPayslips(Stream stream);
        IEnumerable<UserModel> ExtractUsers(Stream stream);
    }
}
