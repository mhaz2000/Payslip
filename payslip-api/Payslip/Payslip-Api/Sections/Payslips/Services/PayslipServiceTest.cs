using Bogus;
using FakeItEasy;
using Payslip.Application.Commands;
using Payslip.Application.Services;
using Payslip.Core.Repositories.Base;
using System.Collections.Generic;
using Xunit;

namespace Payslip_Api.Sections.Payslips.Services
{
    public class PayslipServiceTest
    {
        private readonly PayslipService _payslipService;
        private readonly IUnitOfWork _unitOfWork;


        public PayslipServiceTest()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _payslipService = new PayslipService();
        }

        [Fact]
        public void Should_Create_Payslips()
        {
            //Arrange
            var payslipCommands = GeneratePayslipCommandData(3);

            _payslipService.CreatePayslips(payslipCommands);
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