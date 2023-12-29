namespace Payslip.Core.Repositories.Base
{
    public interface IUnitOfWork
    {
        IFileRepository FileRepository { get; }
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IPayslipRepository PayslipRepository { get; }
        Task<int> CommitAsync();

    }
}
