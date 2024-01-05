using FakeItEasy;
using Payslip.API.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Payslip.Application.Services;
using Payslip.Application.Commands;
using FluentValidation;
using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using Payslip_Api.Fixtures;
using Microsoft.Extensions.Configuration;

namespace Payslip_Api.Sections.Authentications.Actions
{
    public class AuthenticationControllerTest
    {
        private readonly AuthenticationController _authenticationController;
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfiguration _config;

        public AuthenticationControllerTest()
        {
            //Arrange
            _config = Constant.InitConfiguration();

            _authenticationService = A.Fake<IAuthenticationService>();

            var fakeHttpContext = A.Fake<HttpContext>();
            var fakeHttpRequest = A.Fake<HttpRequest>();

            var requestBody = new MemoryStream();
            var writer = new StreamWriter(requestBody);
            writer.Write("Your request content here");
            writer.Flush();
            requestBody.Position = 0;

            A.CallTo(() => fakeHttpRequest.Body).Returns(requestBody);

            A.CallTo(() => fakeHttpRequest.Headers).Returns(
            new HeaderDictionary
            {
                {"Authorization", $"Bearer {Constant.GenerateRandomToken(_config["JwtIssuerOptions:SecretKey"], _config["JwtIssuerOptions:Issuer"], _config["JwtIssuerOptions:Audience"])}"}
            });

            A.CallTo(() => fakeHttpContext.Request).Returns(fakeHttpRequest);
            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext,
            };

            _authenticationController = new AuthenticationController(_authenticationService!);
            _authenticationController.ControllerContext = context;
        }

        #region login

        [Theory]
        [InlineData("1990919677", "113")]
        public async void Should_return_ok_when_credential_is_correct(string nationalCode, string personnelCode)
        {
            // Arrange
            var loginCommand = new LoginCommand(nationalCode, personnelCode);

            // Act
            var response = await _authenticationController.Login(loginCommand);
            var result = (ObjectResult)response;

            // Assert
            response.Should().BeOfType<OkObjectResult>();
            result.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData(null, "110313")]
        public async void Should_return_error_when_username_is_null(string nationalCode, string personnelCode)
        {
            // Arrange
            var loginCommand = new LoginCommand(nationalCode, personnelCode);

            // Assert
            await _authenticationController.Invoking(x => x.Login(loginCommand))
                .Should().ThrowAsync<ValidationException>().WithMessage("نام کاربری الزامی است.");
        }

        [Theory]
        [InlineData("1990919677", null)]
        public async void Should_return_error_when_password_is_null(string nationalCode, string personnelCode)
        {
            // Act
            var loginCommand = new LoginCommand(nationalCode, personnelCode);

            // Assert
            await _authenticationController.Invoking(x => x.Login(loginCommand))
                .Should().ThrowAsync<ValidationException>().WithMessage("رمز عبور الزامی است.");
        }

        [Theory]
        [InlineData(null, null)]
        public async void Should_return_error_when_password_and_username_are_null(string nationalCode, string personnelCode)
        {
            // Act
            var loginCommand = new LoginCommand(nationalCode, personnelCode);

            // Assert
            await _authenticationController.Invoking(x => x.Login(loginCommand))
                .Should().ThrowAsync<ValidationException>().WithMessage("نام کاربری الزامی است.\nرمز عبور الزامی است.");
        }

        #endregion

        #region change password

        [Theory]
        [InlineData("123", "1234")]
        [InlineData("123", "")]
        [InlineData("123", "        ")]
        [InlineData("123", "15  54  ")]
        public void Should_Raise_Error_When_New_Password_Policy_Is_Wrong(string oldPassword, string newPassword)
        {
            //Arrange
            var changePasswordCommand = new ChangePasswordCommand()
            {
                NewPassword = newPassword,
                OldPassword = oldPassword,
            };

            //Act
            var act = async () => await _authenticationController.ChangePassword(changePasswordCommand);
            act.Invoke();

            //Assert
            act.Should().ThrowAsync<ValidationException>();
        }

        [Theory]
        [InlineData("12345678", "87654321")]
        public async void Should_Return_Ok_When_New_Password_Is_Correct(string oldPassword, string newPassword)
        {
            //Arrange
            var changePasswordCommand = new ChangePasswordCommand()
            {
                NewPassword = newPassword,
                OldPassword = oldPassword,
            };

            var act = async () => await _authenticationController.ChangePassword(changePasswordCommand);

            // Assert
            await act.Should().NotThrowAsync<ValidationException>().ConfigureAwait(false);

            A.CallTo(() => _authenticationService.ChangePassword(A<Guid>._, changePasswordCommand))
                .MustHaveHappenedOnceExactly();
        }

        #endregion

        #region change password by admin

        [Fact]
        public void Should_Raise_Exception_When_Inputs_Are_Empty()
        {
            var command = new ChangePasswordByAdminCommand();

            var act = async () => await _authenticationController.ChangePasswordByAdmin(command);

            act.Should().ThrowAsync<ValidationException>().WithMessage("شناسه کاربر الزامی است.\nرمز عبور جدید اجباری است.");
        }

        [Theory]
        [InlineData("   ")]
        [InlineData("  sdfsf ")]
        [InlineData(" 156 ")]
        [InlineData("123 5")]
        [InlineData("1f sg3")]
        public void Should_Raise_Exception_When_Password_Policy_Is_Not_Matched_Invalid_Char(string password)
        {
            var command = new ChangePasswordByAdminCommand()
            {
                UserId = Guid.NewGuid(),
                NewPassword = password
            };

            var act = async () => await _authenticationController.ChangePasswordByAdmin(command);

            act.Should().ThrowAsync<ValidationException>().WithMessage("رمز عبور نمی‌تواند شامل کاراکتر فاصله باشد.");
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("123456d")]
        [InlineData("d4")]
        public void Should_Raise_Exception_When_Password_Policy_Is_Not_Matched_Invalid_Length(string password)
        {
            var command = new ChangePasswordByAdminCommand()
            {
                UserId = Guid.NewGuid(),
                NewPassword = password
            };

            var act = async () => await _authenticationController.ChangePasswordByAdmin(command);

            act.Should().ThrowAsync<ValidationException>().WithMessage("رمز عبور باید حداقل شامل 8 کاراکتر باشد.");
        }
        #endregion
    }
}
