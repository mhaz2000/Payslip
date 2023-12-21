using Payslip.Core.Entities;
using Payslip.Core.Repositories.Base;

namespace Payslip.Core.Repositories
{
    public interface IPayslipRepository : IRepository<UserPayslip>
    {
        IEnumerable<UserPayslip> GetUserPayslips(Guid userId);
    }
}