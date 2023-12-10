using FakeItEasy;
using Payslip.API.Controllers;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Payslip.Application.Services;
using Payslip.Application.Commands;
using FluentValidation;

namespace Payslip_Api.Authentications.Actions
{
    public class AuthenticationControllerTest
    {
        private readonly AuthenticationController _authenticationController;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationControllerTest()
        {
            //Arrange
            _authenticationService = A.Fake<IAuthenticationService>();
            _authenticationController = new AuthenticationController(_authenticationService!);
        }


        [Theory]
        [InlineData("199677", "113")]
        public async void Should_return_error_when_username_is_wrong(string nationalCode, string personnelCode)
        {
            // Arrange
            var loginCommand = new LoginCommand(nationalCode, personnelCode);

            // Act
            var response = await _authenticationController.Login(loginCommand);
            var result = (ObjectResult)response;



            // Assert
            result.StatusCode.Should().Be(200);
            response.Should().BeOfType<OkObjectResult>();
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
    }
}
