using Microsoft.AspNetCore.Identity;
using Payslip.Application.Base;
using Payslip.Application.Commands;
using Payslip.Application.DTOs;
using Payslip.Application.Helpers.TokenHelpers;
using Payslip.Core.Entities;
using Payslip.Core.Repositories.Base;

namespace Payslip.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly UserManager<User> _userManager;
        public AuthenticationService(IUnitOfWork unitOfWork, ITokenGenerator tokenGenerator, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _tokenGenerator = tokenGenerator;
            _userManager = userManager;
        }

        public async Task ChangePassword(Guid userId, ChangePasswordCommand command)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربر مورد نظر یافت نشد.");

            if(!(await _userManager.CheckPasswordAsync(user, command.OldPassword)))
                throw new ManagedException("رمز عبور قبلی اشتباه است.");

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, command.NewPassword);
            await _unitOfWork.CommitAsync();
        }

        public async Task ChangePasswordByAdmin(Guid userId, ChangePasswordByAdminCommand command)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربر مورد نظر یافت نشد.");

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, command.NewPassword);

            await _unitOfWork.CommitAsync();
        }

        public async Task<UserLoginDTO> Login(LoginCommand loginCommand, JwtIssuerOptionsModel jwtIssuerOptions)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(c => c.UserName == loginCommand.Username);
            if (user is null)
                throw new ManagedException("نام کاربری یا رمز عبور اشتباه است.");

            if (!await _userManager.CheckPasswordAsync(user, loginCommand.Password))
                throw new ManagedException("نام کاربری یا رمز عبور اشتباه است.");

            var userRoles = _unitOfWork.RoleRepository.GetUserRoles(user).ToList();
            var token = _tokenGenerator.TokenGeneration(user, jwtIssuerOptions, userRoles);

            return new UserLoginDTO()
            {
                IsAdmin = userRoles.Any(c => c.Name.ToLower() == "admin"),
                ExpiresIn = token.expires_in,
                AuthToken = token.AuthToken,
                RefreshToken = token.RefreshToken,
                FullName = user.FirstName + " " + user.LastName
            };
        }
    }
}
