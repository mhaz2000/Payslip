using Bogus;
using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payslip.API.Base;
using Payslip.API.Controllers;
using Payslip.API.Helpers;
using Payslip.Application.Commands;
using Payslip.Application.DTOs;
using Payslip.Application.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Payslip_Api.Sections.Users.Actions
{
    public class UserControllerTests
    {
        private readonly UsersController _userController;
        private readonly IUserService _userService;
        public UserControllerTests()
        {
            _userService = A.Fake<IUserService>();
            _userController = new UsersController(_userService, A.Fake<IExcelHelpler>());
        }

        #region Get Users
        [Theory]
        [InlineData(10)]
        public async Task Get_User_Should_Return_Ok_Result(int skip)
        {
            //Arrange
            var generatedUsers = GenerateUsers(5);
            A.CallTo(() => _userService.GetUsers(A<int>._, A<string>._)).Returns((generatedUsers, 5));

            //Act
            var response = await _userController.GetUsers(skip);
            var result = (OkObjectResult)response;
            var data = result.Value as ResponseModel;
            var users = data!.Data as IEnumerable<UserDTO>;

            response.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            data.Total.Should().Be(5);
            users.Should().HaveCount(generatedUsers.Count());
        }

        [Theory]
        [InlineData(null)]
        public async Task Get_User_Should_Return_Ok_Result_Even_When_Query_String_Is_Null(int? skip)
        {
            //Arrange
            var generatedUsers = GenerateUsers(5);
            A.CallTo(() => _userService.GetUsers(A<int>._, A<string>._)).Returns((generatedUsers, 5));

            //Act
            var response = await _userController.GetUsers(skip);
            var result = (OkObjectResult)response;
            var data = result.Value as ResponseModel;
            var users = data!.Data as IEnumerable<UserDTO>;

            response.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            data.Total.Should().Be(5);
            users.Should().HaveCount(generatedUsers.Count());
        }

        #endregion

        #region Create User

        [Fact]
        public async void Create_User_Should_Return_Ok_Result_When_Request_Is_Valid()
        {
            //Arrange
            var command = new UserCreateCommand()
            {
                CardNumber = "15054152",
                FirstName = "محمد",
                LastName = "حبیب اله زاده",
                NationalCode = "1990919677"
            };

            //Act
            var response = await _userController.CreateUser(command);

            //Assert
            A.CallTo(() => _userService.CreateUser(command)).MustHaveHappenedOnceExactly();

            response.Should().NotBeNull();
            response.Should().BeAssignableTo<OkResult>();
        }

        [Fact]
        public async void Should_Raise_Exception_When_Inputs_Are_Empty()
        {
            //Arrange
            var command = new UserCreateCommand()
            {
                CardNumber = "",
                FirstName = "",
                LastName = "",
                NationalCode = ""
            };

            await _userController.Invoking(x => x.CreateUser(command)).Should().ThrowAsync<FluentValidation.ValidationException>()
                .WithMessage("نام الزامی است.\nنام خانوادگی الزامی است.\nکد ملی الزامی است.\nشماره کارت الزامی است.");
        }

        [Theory]
        [InlineData("1231dfdf", "153445j354")]
        [InlineData("aaaaaa", "bbbbbbb")]
        public async void Should_Raise_Exception_When_NationalCode_Or_CardNumber_Is_Wrong(string nationalCode, string cardNumber)
        {
            //Arrange
            var command = new UserCreateCommand()
            {
                CardNumber = cardNumber,
                FirstName = "محمد",
                LastName = "حبیب اله زاده",
                NationalCode = nationalCode
            };

            await _userController.Invoking(x => x.CreateUser(command)).Should().ThrowAsync<FluentValidation.ValidationException>()
                .WithMessage("فرمت کد ملی صحیح نیست.\nفرمت شماره کارت صحیح نیست.");
        }

        #endregion

        #region Toggle User Activation

        [Fact]
        public async void Toggle_Activation_Should_Return_Ok()
        {
            //Act
            var response = await _userController.ToggleActivation(Guid.NewGuid());

            //Assert
            A.CallTo(() => _userService.ToggleActivation(A<Guid>._)).MustHaveHappenedOnceExactly();

            response.Should().NotBeNull();
            response.Should().BeAssignableTo<OkResult>();
        }

        #endregion

        #region Delete User

        [Fact]
        public async void Delete_User_Should_Return_Ok()
        {
            //Act
            var response = await _userController.Delelte(Guid.NewGuid());

            //Assert
            A.CallTo(() => _userService.RemoveUser(A<Guid>._)).MustHaveHappenedOnceExactly();

            response.Should().NotBeNull();
            response.Should().BeAssignableTo<OkResult>();
        }

        #endregion

        #region Import Users Excel

        [Fact]
        public async Task Should_Import_User_ExcelAsync()
        {
            var fakeDTO = new ImportUserExcelCommmand()
            {
                File = A.Fake<IFormFile>(),
            };

            //Act
            var response = await _userController.ImportUsers(fakeDTO);
            var result = (OkResult)response;

            response.Should().NotBeNull();
            response.Should().BeOfType<OkResult>();
            result.StatusCode.Should().Be(200);
        }

        #endregion

        private IEnumerable<UserDTO> GenerateUsers(int count)
        {
            Random random = new Random();

            var faker = new Faker<UserDTO>()
                .RuleFor(c => c.FirstName, f => f.Person.FirstName)
                .RuleFor(c => c.LastName, f => f.Person.LastName)
                .RuleFor(c => c.NationalCode, f => random.NextInt64(1111111111, 9999999999).ToString())
                .RuleFor(c => c.CardNumber, f => random.NextInt64(111111, 999999).ToString())
                .RuleFor(c => c.Id, f => Guid.NewGuid());

            return faker.Generate(count);
        }
    }
}
