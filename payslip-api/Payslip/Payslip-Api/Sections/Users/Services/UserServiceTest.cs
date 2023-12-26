using AutoMapper;
using Bogus;
using FakeItEasy;
using FluentAssertions;
using Payslip.API.Base;
using Payslip.Application.DTOs;
using Payslip.Application.Services;
using Payslip.Core.Entities;
using Payslip.Core.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Payslip_Api.Sections.Users.Services
{
    public class UserServiceTest
    {
        private readonly UserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserServiceTest()
        {
            _mapper = A.Fake<IMapper>();
            _unitOfWork = A.Fake<IUnitOfWork>();
            _userService = new UserService(_unitOfWork, _mapper);
        }

        [Theory]
        [InlineData(5)]
        public void Should_Get_Users(int skip)
        {
            //Arrange 
            var dbUsers = GenerateUsers(20).AsQueryable();
            A.CallTo(() => _unitOfWork.UserRepository.Where(A<Expression<Func<User, bool>>>._)).Returns(dbUsers.AsQueryable());

            var data = _userService.GetUsers(skip);
            A.CallTo(() => _unitOfWork.UserRepository.Where(A<Expression<Func<User, bool>>>._)).MustHaveHappened();
            A.CallTo(() => _mapper.Map<IEnumerable<UserDTO>>(A<IEnumerable<User>>._)).MustHaveHappenedOnceExactly();


            data.Total.Should().Be(20);
            data.Users.Should().NotBeNull();
            data.Users.Should().BeAssignableTo<IEnumerable<UserDTO>>();
        }

        private IEnumerable<User> GenerateUsers(int count)
        {
            Random random = new Random();

            var faker = new Faker<User>()
                .RuleFor(c => c.IsActive, f => random.NextInt64(0, 3) > 1 ? true : false)
                .RuleFor(c => c.FirstName, f => f.Person.FirstName)
                .RuleFor(c => c.LastName, f => f.Person.LastName)
                .RuleFor(c => c.NationalCode, f => random.NextInt64(1111111111, 9999999999).ToString())
                .RuleFor(c => c.CardNumber, f => random.NextInt64(111111, 999999).ToString())
                .RuleFor(c => c.Id, f => Guid.NewGuid());

            return faker.Generate(count);
        }
    }
}
