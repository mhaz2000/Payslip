using Microsoft.EntityFrameworkCore;
using Payslip.Core.Entities;
using Payslip.Core.Repositories;
using Payslip.Infrastructure.Data;
using Payslip.Infrastructure.Repositories.Base;

namespace Payslip.Infrastructure.Repositories
{
    public class PayslipRepository : Repository<UserPayslip>, IPayslipRepository
    {
        public PayslipRepository(DataContext context) : base(context)
        {

        }

        public IEnumerable<UserPayslip> GetUserPayslips(Guid userId)
        {
            return Context.UserPayslips.Include(c => c.User).Where(c => c.User!.Id == userId);
        }
    }
}
