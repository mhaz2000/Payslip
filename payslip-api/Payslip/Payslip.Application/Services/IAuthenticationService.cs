using Payslip.Application.Base;
using Payslip.Application.Commands;
using Payslip.Application.DTOs;

namespace Payslip.Application.Services
{
    public interface IAuthenticationService
    {
        Task<UserLoginDTO> Login(LoginCommand loginCommand, JwtIssuerOptionsModel jwtIssuerOptions);
    }
}
