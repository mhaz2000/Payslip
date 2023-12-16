using Payslip.Core.Repositories;
using Payslip.Core.Repositories.Base;
using Payslip.Infrastructure.Data;

namespace Payslip.Infrastructure.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;


        private RoleRepository? _roleRepository;
        private UserRepository? _userRepository;

        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);

        public IRoleRepository RoleRepository => _roleRepository ?? new RoleRepository(_context);

        public UnitOfWork(DataContext context) => _context = context;


        public async Task<int> CommitAsync()
        {
            var result = await _context.SaveChangesAsync();
            Dispose();
            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
