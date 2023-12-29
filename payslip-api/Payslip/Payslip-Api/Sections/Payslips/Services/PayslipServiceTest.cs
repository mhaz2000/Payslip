using AutoMapper;
using Bogus;
using FakeItEasy;
using FluentAssertions;
using Payslip.Application.Base;
using Payslip.Application.Commands;
using Payslip.Application.DTOs;
using Payslip.Application.Helpers;
using Payslip.Application.Services;
using Payslip.Core.Entities;
using Payslip.Core.Enums;
using Payslip.Core.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Payslip_Api.Sections.Payslips.Services
{
    public class PayslipServiceTest
    {
        private readonly PayslipService _payslipService;
        private readonly IUnitOfWork _unitOfWork;


        public PayslipServiceTest()
        {
            //Arrange
            var mapper = A.Fake<IMapper>();
            _unitOfWork = A.Fake<IUnitOfWork>();
            _payslipService = new PayslipService(_unitOfWork, mapper);
        }

        [Fact]
        public async Task Should_Create_Payslips()
        {
            //Arrange
            var payslipCommands = GeneratePayslipCommandData(3);

            //Act
            var act = async () => await _payslipService.CreatePayslips(payslipCommands, 1400, 10, Guid.NewGuid());

            await act.Should().NotThrowAsync();

            A.CallTo(() => _unitOfWork.PayslipRepository.AddRangeAsync(A<IEnumerable<UserPayslip>>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();

        }

        [Fact]
        public async Task Should_Throw_Exception_When_PayslipCommand_Is_Empty()
        {
            //Arrange
            var payslipCommands = new List<PayslipCommand>();

            //Act
            var act = async () => await _payslipService.CreatePayslips(payslipCommands, 1400, 5, Guid.NewGuid());

            await act.Should().ThrowExactlyAsync<ManagedException>().WithMessage("خطایی در خواندن مقادیر اکسل ارسالی رخ داده است.");

            A.CallTo(() => _unitOfWork.PayslipRepository.AddRangeAsync(A<IEnumerable<UserPayslip>>._)).MustNotHaveHappened();
            A.CallTo(() => _unitOfWork.CommitAsync()).MustNotHaveHappened();

        }

        [Fact]
        public async Task Should_Return_User_Payslips_Wages()
        {
            //Arrange
            var payslips = new List<UserPayslip>()
            {
                new UserPayslip(){Year = 1401, Month=Month.Esfand},
                new UserPayslip(){Year = 1402, Month=Month.Farvardin},
                new UserPayslip(){Year = 1402, Month=Month.Ordibehesht},
            };

            IEnumerable<UserPayslipWagesDTO> expectedValue = new List<UserPayslipWagesDTO>()
            {
                new UserPayslipWagesDTO ()
                {
                    Year = 1401,
                    Months = new List<Month>() { Month.Esfand }
                },
                new UserPayslipWagesDTO ()
                {
                    Year = 1402,
                    Months = new List<Month>() { Month.Farvardin, Month.Ordibehesht}
                },
            };

            //Act
            A.CallTo(() => _unitOfWork.UserRepository.GetByIdAsync(A<Guid>._))
                .Returns(new User());

            A.CallTo(() => _unitOfWork.PayslipRepository.GetUserPayslips(A<Guid>._))
                .Returns(payslips);

            var act = async () => await _payslipService.GetUserWages(Guid.NewGuid());

            //Assert
            var result = await act.Invoke();
            result.Should().HaveCount(2);
        }

        private static List<PayslipCommand> GeneratePayslipCommandData(int count)
        {
            var faker = new Faker<PayslipCommand>()
                .RuleFor(c => c.Bank, f => f.Name.FindName())
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName());

            return faker.Generate(count);
        }
    }
}