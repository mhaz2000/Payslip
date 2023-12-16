using Microsoft.AspNetCore.Identity;
using Payslip.Core.Entities;
using Payslip.Core.Repositories;
using Payslip.Infrastructure.Data;
using Payslip.Infrastructure.Repositories.Base;

namespace Payslip.Infrastructure.Repositories
{
    public class RoleRepository : Repository<IdentityRole<Guid>>, IRoleRepository
    {

        public RoleRepository(DataContext context) : base(context)
        {

        }
        public IQueryable<IdentityRole<Guid>> GetUserRoles(User user)
        {
            var roles = Context.UserRoles.Where(c => c.UserId == user.Id).Select(s => s.RoleId);
            return Context.Roles.Where(c => roles.Contains(c.Id));
        }
    }
}
