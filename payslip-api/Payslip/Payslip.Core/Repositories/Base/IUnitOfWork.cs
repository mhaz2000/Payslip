namespace Payslip.Core.Repositories.Base
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        Task<int> CommitAsync();

    }
}
