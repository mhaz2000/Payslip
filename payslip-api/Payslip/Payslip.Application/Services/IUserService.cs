using Payslip.Application.Commands;
using Payslip.Application.DTOs;

namespace Payslip.Application.Services
{
    public interface IUserService
    {
        Task CreateUser(UserCreateCommand command);
        Task CreateUsers(IEnumerable<UserCreateCommand> users);
        (IEnumerable<UserDTO> Users, int Total) GetUsers(int skip, string search);
        Task RemoveUser(Guid userId);
        Task ToggleActivation(Guid userId);
    }
}
