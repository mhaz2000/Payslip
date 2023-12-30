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
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Payslip_Api.Sections.Payslips.Services
{
    public class PayslipServiceTest
    {
        private readonly PayslipService _payslipService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;


        public PayslipServiceTest()
        {
            //Arrange
            var mapper = A.Fake<IMapper>();
            _unitOfWork = A.Fake<IUnitOfWork>();
            _fileService = A.Fake<IFileService>();
            _payslipService = new PayslipService(_unitOfWork, mapper, _fileService);
        }

        #region Create Payslips

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

        #endregion

        #region Get User payslips Wages

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
                    Months = new List<int>() { Month.Esfand.GetHashCode() }
                },
                new UserPayslipWagesDTO ()
                {
                    Year = 1402,
                    Months = new List<int>() { Month.Farvardin.GetHashCode(), Month.Ordibehesht.GetHashCode() }
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

        #endregion

        #region Get All Payslips

        [Fact]
        public void Should_Get_All_Payslips()
        {
            //Arrange
            var data = GeneratePayslipData(5);
            var expectedData = data.GroupBy(c => new { c.Year, c.Month, c.FileId }).Select(s => new PayslipDTO()
            {
                FileId = s.Key.FileId,
                Month = s.Key.Month.GetDescription(),
                Year = s.Key.Year,
                UploadedDate = s.FirstOrDefault()!.CreatedAt
            });


            A.CallTo(() => _unitOfWork.PayslipRepository.OrderByDescending(A<Expression<Func<UserPayslip, object>>>._))
                .Returns(data.AsQueryable());

            var payslips = _payslipService.GetPayslips(0);

            payslips.Total.Should().Be(5);
            payslips.Payslips.Should().BeEquivalentTo(expectedData);
        }

        #endregion

        #region Remove Payslip

        [Fact]
        public async void Should_Throw_Error_Whtn_File_Is_Not_Found()
        {
            //Arrange
            var id = Guid.NewGuid();
            A.CallTo(() => _unitOfWork.FileRepository.AnyAsync(A<Expression<Func<FileModel, bool>>>._)).Returns(false);

            //Act
            await _payslipService.Invoking(c=> c.RemovePayslip(id)).Should().ThrowAsync<ManagedException>().WithMessage("فیش مورد نظر یافت نشد.");
        }

        [Fact]
        public async void Should_Remove_Payslip()
        {
            //Arrange
            var id = Guid.NewGuid();
            A.CallTo(() => _unitOfWork.FileRepository.AnyAsync(A<Expression<Func<FileModel, bool>>>._)).Returns(true);

            //Act
            await _payslipService.RemovePayslip(id);
            A.CallTo(() => _fileService.RemoveFile(id)).MustHaveHappened();
            A.CallTo(() => _unitOfWork.CommitAsync()).MustHaveHappened();
        }

        #endregion

        private static List<PayslipCommand> GeneratePayslipCommandData(int count)
        {
            var faker = new Faker<PayslipCommand>()
                .RuleFor(c => c.Bank, f => f.Name.FindName())
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName());

            return faker.Generate(count);
        }

        private static List<UserPayslip> GeneratePayslipData(int count)
        {
            Random reandom = new Random();
            var months = new Month[] { Month.Farvardin, Month.Azar, Month.Mehr, Month.Dey };

            var faker = new Faker<UserPayslip>()
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Year, f => reandom.Next(1400, 1410))
                .RuleFor(c => c.Month, f => f.PickRandom(months))
                .RuleFor(c => c.CreatedAt, f => DateTime.Now)
                .RuleFor(c => c.FileId, f => Guid.NewGuid());

            return faker.Generate(count);
        }
    }
}