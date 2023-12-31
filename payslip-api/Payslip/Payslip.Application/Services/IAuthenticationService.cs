﻿using Payslip.Application.Base;
using Payslip.Application.Commands;
using Payslip.Application.DTOs;

namespace Payslip.Application.Services
{
    public interface IAuthenticationService
    {
        Task<UserLoginDTO> Login(LoginCommand loginCommand, JwtIssuerOptionsModel jwtIssuerOptions);
        Task ChangePassword(Guid userId, ChangePasswordCommand command);
        Task ChangePasswordByAdmin(Guid userId, ChangePasswordByAdminCommand command);
    }
}
