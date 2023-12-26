using Payslip.Application.DTOs;

namespace Payslip.Application.Services
{
    public interface IUserService
    {
        (IEnumerable<UserDTO> Users, int Total) GetUsers(int skip);
    }
}
