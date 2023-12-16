using Microsoft.AspNetCore.Identity;
using Payslip.Core.Entities;
using Payslip.Core.Repositories.Base;

namespace Payslip.Core.Repositories
{
    public interface IRoleRepository : IRepository<IdentityRole<Guid>>
    {
        IQueryable<IdentityRole<Guid>> GetUserRoles(User user);
    }
}
