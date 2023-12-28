using AutoMapper;
using Bogus;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Payslip.API.Base;
using Payslip.Application.Base;
using Payslip.Application.Commands;
using Payslip.Application.DTOs;
using Payslip.Application.Services;
using Payslip.Core.Entities;
using Payslip.Core.Repositories.Base;
using Payslip.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Payslip_Api.Sections.Users.Services
{
    public class UserServiceTest
    {
        private readonly UserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserServiceTest()
        {
            _userManager = A.Fake<UserManager<User>>();
            _mapper = A.Fake<IMapper>();
            _unitOfWork = A.Fake<IUnitOfWork>();
            _userService = new UserService(_unitOfWork, _mapper, _userManager);
        }

        #region Get Users

        [Theory]
        [InlineData(5)]
        public void Should_Get_Users(int skip)
        {
            //Arrange 
            var dbUsers = GenerateUsers(20).AsQueryable();
            A.CallTo(() => _unitOfWork.UserRepository.Where(A<Expression<Func<User, bool>>>._)).Returns(dbUsers.AsQueryable());

            var data = _userService.GetUsers(skip, string.Empty);
            A.CallTo(() => _unitOfWork.UserRepository.Where(A<Expression<Func<User, bool>>>._)).MustHaveHappened();
            A.CallTo(() => _mapper.Map<IEnumerable<UserDTO>>(A<IEnumerable<User>>._)).MustHaveHappenedOnceExactly();


            data.Total.Should().Be(20);
            data.Users.Should().NotBeNull();
            data.Users.Should().BeAssignableTo<IEnumerable<UserDTO>>();
        }

        #endregion

        #region Create Usr

        [Fact]
        public async void Create_User_Should_Raise_Error_When_NationalCode_is_repetitive()
        {
            var command = new UserCreateCommand()
            {
                CardNumber = "123145",
                FirstName = "محمد",
                LastName = "حبیب اله زاده",
                NationalCode = "1111111111"
            };

            A.CallTo(() => _unitOfWork.UserRepository.AnyAsync(A<Expression<Func<User, bool>>>._))
                .Returns(Task.FromResult(true));

            await _userService.Invoking(c => c.CreateUser(command)).Should().ThrowAsync<ManagedException>().WithMessage("کد ملی تکراری است.");
        }

        [Fact]
        public async void Create_User_Should_Raise_Error_When_CardNumber_is_repetitive()
        {
            var command = new UserCreateCommand()
            {
                CardNumber = "123145",
                FirstName = "محمد",
                LastName = "حبیب اله زاده",
                NationalCode = "1111111111"
            };

            A.CallTo(() => _unitOfWork.UserRepository.AnyAsync(A<Expression<Func<User, bool>>>._))
                .ReturnsNextFromSequence(false, true);

            await _userService.Invoking(c => c.CreateUser(command)).Should().ThrowAsync<ManagedException>().WithMessage("شماره کارت تکراری است.");
        }

        [Fact]
        public async void Create_User_Should_Work_Correctly()
        {
            var command = new UserCreateCommand()
            {
                CardNumber = "123145",
                FirstName = "محمد",
                LastName = "حبیب اله زاده",
                NationalCode = "1111111111"
            };

            A.CallTo(() => _unitOfWork.UserRepository.AnyAsync(A<Expression<Func<User, bool>>>._))
                .ReturnsNextFromSequence(false);

            await _userService.Invoking(c => c.CreateUser(command)).Should().NotThrowAsync<ManagedException>();

            A.CallTo(() => _userManager.CreateAsync(A<User>._, A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userManager.AddToRoleAsync(A<User>._, A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
        }

        #endregion

        #region Toggle Activation

        [Fact]
        public async void Toggle_Activation_Should_Work_Correctly()
        {
            A.CallTo(() => _unitOfWork.UserRepository.FirstOrDefaultAsync(A<Expression<Func<User, bool>>>.Ignored)).Returns(Task.FromResult(new User()));

            await _userService.ToggleActivation(Guid.NewGuid());

            A.CallTo(() => _unitOfWork.UserRepository.FirstOrDefaultAsync(A<Expression<Func<User,bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void Toggle_Activation_Should_Raise_Error_When_User_Does_Not_Exist()
        {
            User user = null;
            A.CallTo(() => _unitOfWork.UserRepository.FirstOrDefaultAsync(A<Expression<Func<User, bool>>>.Ignored)).Returns(Task.FromResult(user)!);

            await _userService.Invoking(c=> c.ToggleActivation(Guid.NewGuid())).Should().ThrowAsync<ManagedException>().WithMessage("کاربر مورد نظر یافت نشد.");
            A.CallTo(() => _unitOfWork.UserRepository.FirstOrDefaultAsync(A<Expression<Func<User, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CommitAsync()).MustNotHaveHappened();
        }

        #endregion

        #region Delete User

        [Fact]
        public async void Delete_User_Should_Work_Correctly()
        {
            A.CallTo(() => _unitOfWork.UserRepository.FirstOrDefaultAsync(A<Expression<Func<User, bool>>>.Ignored)).Returns(Task.FromResult(new User()));

            await _userService.RemoveUser(Guid.NewGuid());

            A.CallTo(() => _unitOfWork.UserRepository.FirstOrDefaultAsync(A<Expression<Func<User, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userManager.DeleteAsync(A<User>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void Delete_User_Should_Raise_Error_When_User_Does_Not_Exist()
        {
            User user = null;
            A.CallTo(() => _unitOfWork.UserRepository.FirstOrDefaultAsync(A<Expression<Func<User, bool>>>.Ignored)).Returns(Task.FromResult(user)!);

            await _userService.Invoking(c => c.RemoveUser(Guid.NewGuid())).Should().ThrowAsync<ManagedException>().WithMessage("کاربر مورد نظر یافت نشد.");
            A.CallTo(() => _unitOfWork.UserRepository.FirstOrDefaultAsync(A<Expression<Func<User, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userManager.DeleteAsync(A<User>._)).MustNotHaveHappened();
            A.CallTo(() => _unitOfWork.CommitAsync()).MustNotHaveHappened();
        }

        #endregion

        private IEnumerable<User> GenerateUsers(int count)
        {
            Random random = new Random();

            var faker = new Faker<User>()
                .RuleFor(c => c.IsActive, f => random.NextInt64(0, 3) > 1 ? true : false)
                .RuleFor(c => c.FirstName, f => f.Person.FirstName)
                .RuleFor(c => c.LastName, f => f.Person.LastName)
                .RuleFor(c => c.IsActive, f => true)
                .RuleFor(c => c.NationalCode, f => random.NextInt64(1111111111, 9999999999).ToString())
                .RuleFor(c => c.CardNumber, f => random.NextInt64(111111, 999999).ToString())
                .RuleFor(c => c.Id, f => Guid.NewGuid());

            return faker.Generate(count);
        }
    }
}
