using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Payslip.Application.Base;
using Payslip.Application.Commands;
using Payslip.Application.Helpers.TokenHelpers;
using Payslip.Application.Services;
using Payslip.Core.Entities;
using Payslip.Core.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Payslip_Api.Sections.Authentications.Services
{
    public class AuthenticationServiceTest
    {
        private readonly AuthenticationService _authenticationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly UserManager<User> _userManager;

        public AuthenticationServiceTest()
        {
            //Arrange
            _unitOfWork = A.Fake<IUnitOfWork>();
            _tokenGenerator = A.Fake<ITokenGenerator>();
            _userManager = A.Fake<UserManager<User>>();
            _authenticationService = new AuthenticationService(_unitOfWork, _tokenGenerator, _userManager);
        }

        #region Login
        [Fact]
        public async Task Should_Raise_Error_When_Password_is_Wrong()
        {
            //Arrange
            var unValidPassword = "unValidPassword";
            var password = "validPassword";
            var username = "validUsername";

            var loginCommand = new LoginCommand(username, unValidPassword);
            var user = new User(username, "محمد", "حبیب اله زاده", "1990919677", "110313");

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(user, password);
            user.PasswordHash = hashedPassword;

            var jwtIssurOptions = new JwtIssuerOptionsModel();

            A.CallTo(() => _unitOfWork.UserRepository.FirstOrDefaultAsync(A<Expression<Func<User, bool>>>._))
                .Returns(user);

            A.CallTo(() => _userManager.CheckPasswordAsync(user, loginCommand.Password))
                .Returns(false);

            // Act
            var act = async () => await _authenticationService.Login(loginCommand, jwtIssurOptions);

            await act.Should().ThrowAsync<ManagedException>();
            await act.Should().ThrowAsync<ManagedException>().WithMessage("نام کاربری یا رمز عبور اشتباه است.");
        }

        [Fact]
        public async Task Should_Raise_Error_When_Username_Id_Wrong()
        {
            //Arrange
            var unValidUsername = "validPassword";
            var password = "validPassword";

            var loginCommand = new LoginCommand(unValidUsername, password);
            User user = null;

            var jwtIssurOptions = new JwtIssuerOptionsModel();

            A.CallTo(() => _unitOfWork.UserRepository.FirstOrDefaultAsync(A<Expression<Func<User, bool>>>._))!
                .Returns(user);

            // Act
            var act = async () => await _authenticationService.Login(loginCommand, jwtIssurOptions);

            await act.Should().ThrowAsync<ManagedException>();
            await act.Should().ThrowAsync<ManagedException>().WithMessage("نام کاربری یا رمز عبور اشتباه است.");
        }

        [Fact]
        public async Task Should_Returns_UserLoginDTO_When_Credential_Is_ValidAsync()
        {
            //Arrange
            var password = "validPassword";
            var username = "validUsername";
            var loginCommand = new LoginCommand(username, password);
            var user = new User(username, "محمد", "حبیب اله زاده", "1990919677", "110313");

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(user, password);
            user.PasswordHash = hashedPassword;

            var tokenResult = new JwTToken
            {
                expires_in = 3600,
                AuthToken = "fakeAuthToken",
                RefreshToken = "fakeRefreshToken",
            };
            var jwtIssurOptions = new JwtIssuerOptionsModel();
            var roles = new List<IdentityRole<Guid>>();

            A.CallTo(() => _unitOfWork.UserRepository.FirstOrDefaultAsync(A<Expression<Func<User, bool>>>._))
                .Returns(user);

            A.CallTo(() => _userManager.CheckPasswordAsync(user, loginCommand.Password))
                .Returns(true);

            A.CallTo(() => _tokenGenerator.TokenGeneration(user, jwtIssurOptions, roles))
                .Returns(tokenResult);

            // Act
            var result = await _authenticationService.Login(loginCommand, jwtIssurOptions);

            result.Should().NotBe(null);
            result.ExpiresIn.Should().Be(tokenResult.expires_in);
            result.AuthToken.Should().Be(tokenResult.AuthToken);
            result.RefreshToken.Should().Be(tokenResult.RefreshToken);
            result.IsAdmin.Should().BeFalse();
        }
        #endregion

        #region Change Password

        [Fact]
        public void Should_Raise_Error_When_Old_Password_Does_not_Match()
        {
            //Arrange
            var command = new ChangePasswordCommand()
            {
                OldPassword = "123",
                NewPassword = "1234"
            };
            var userId = Guid.NewGuid();
            A.CallTo(()=> _userManager.CheckPasswordAsync(A<User>._, command.OldPassword)).Returns(false);

            //Act
            var act = async () => await _authenticationService.ChangePassword(userId,command);
            act.Invoke();

            //Assert
            A.CallTo(() => _unitOfWork.UserRepository.GetByIdAsync(userId)).MustHaveHappenedOnceExactly();
            act.Should().ThrowAsync<ManagedException>().WithMessage("رمز عبور قبلی اشتباه است.");
        }

        [Theory]
        [InlineData("123", "12345678")]
        public void Should_Change_Password(string oldPassword, string newPassword)
        {
            //Arrange
            var command = new ChangePasswordCommand()
            {
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            var userId = Guid.NewGuid();

            A.CallTo(() => _unitOfWork.UserRepository.GetByIdAsync(userId)).Returns(new User());
            A.CallTo(() => _userManager.CheckPasswordAsync(A<User>._, command.OldPassword)).Returns(true);

            //Act
            var act = async () => await _authenticationService.ChangePassword(userId, command);
            act.Invoke();

            //Assert
            A.CallTo(() => _userManager.PasswordHasher.HashPassword(A<User>._, command.NewPassword)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
        }
        #endregion

        #region Change password by admin

        [Fact]
        public async void Should_Raise_Error_When_User_Cannot_Be_Found()
        {
            A.CallTo(() => _unitOfWork.UserRepository.GetByIdAsync(A<Guid>._)).Returns(null);
            var act = async () => await _authenticationService.ChangePasswordByAdmin(Guid.NewGuid(), new ChangePasswordByAdminCommand());

            await act.Should().ThrowAsync<ManagedException>().WithMessage("کاربر مورد نظر یافت نشد.");
        }

        [Fact]
        public async void Should_Change_User_Password()
        {
            A.CallTo(() => _unitOfWork.UserRepository.GetByIdAsync(A<Guid>._)).Returns(new User());

            await _authenticationService.ChangePasswordByAdmin(Guid.NewGuid(), new ChangePasswordByAdminCommand());

            A.CallTo(() => _userManager.PasswordHasher.HashPassword(A<User>._, A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
        }

        #endregion
    }
}
