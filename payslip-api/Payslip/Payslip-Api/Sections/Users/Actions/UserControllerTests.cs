using Bogus;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Payslip.API.Base;
using Payslip.API.Controllers;
using Payslip.Application.DTOs;
using Payslip.Application.Services;
using Payslip.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            _userController = new UsersController(_userService);
        }
        [Theory]
        [InlineData(10)]
        public void Should_Return_Ok_Result(int skip)
        {
            //Arrange
            var generatedUsers = GenerateUsers(5);
            A.CallTo(() => _userService.GetUsers(A<int>._)).Returns((generatedUsers,5));

            //Act
            var response = _userController.GetUsers(skip);
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
        public void Should_Return_Ok_Result_Even_When_Query_String_Is_Null(int? skip)
        {
            //Arrange
            var generatedUsers = GenerateUsers(5);
            A.CallTo(() => _userService.GetUsers(A<int>._)).Returns((generatedUsers,5));

            //Act
            var response = _userController.GetUsers(skip);
            var result = (OkObjectResult)response;
            var data = result.Value as ResponseModel;
            var users = data!.Data as IEnumerable<UserDTO>;

            response.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            data.Total.Should().Be(5);
            users.Should().HaveCount(generatedUsers.Count());
        }

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
