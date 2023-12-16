using Payslip.Core.Entities;
using Payslip.Core.Repositories;
using Payslip.Infrastructure.Data;
using Payslip.Infrastructure.Repositories.Base;

namespace Payslip.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext dataContext) : base(dataContext) 
        {

        }
    }
}
