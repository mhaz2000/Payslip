using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payslip.API.Controllers;
using Payslip.API.Helpers;
using Payslip.Application.DTOs;
using Payslip.Application.Services;
using Xunit;

namespace Payslip_Api.Sections.Payslips.Actions
{
    public class PayslipsControllerTest
    {
        private readonly PayslipsController _payslipsController;
        public PayslipsControllerTest()
        {
            var payslipService = A.Fake<IPayslipService>();
            var payslipHelper = A.Fake<IPayslipExtractorHelpler>();
            _payslipsController = new PayslipsController(payslipService, payslipHelper);
        }

        [Fact]
        public async void Should_return_ok_when_file_uploaded()
        {
            //Arrange
            var fakeDTO = new UploadPayslipCommand()
            {
                File = A.Fake<IFormFile>(),
            };

            //Act
            var response = await _payslipsController.UploadPayslipFile(fakeDTO, 1402,8);
            var result = (OkResult)response;

            //Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<OkResult>();
            result.StatusCode.Should().Be(200);

        }
    }
}
