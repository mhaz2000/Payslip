using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Payslip.API.Controllers;
using Payslip.API.Helpers;
using Payslip.Application.DTOs;
using Payslip.Application.Services;
using Payslip.Core.Enums;
using Payslip_Api.Fixtures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Payslip_Api.Sections.Payslips.Actions
{
    public class PayslipsControllerTest
    {
        private readonly IPayslipService _payslipService;
        private readonly PayslipsController _payslipsController;
        private readonly IConfiguration _config;
        public PayslipsControllerTest()
        {
            _config = Constant.InitConfiguration();

            _payslipService = A.Fake<IPayslipService>();
            var payslipHelper = A.Fake<IPayslipExtractorHelpler>();

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

            _payslipsController = new PayslipsController(_payslipService, payslipHelper);
            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext,
            };

            _payslipsController.ControllerContext = context;
        }

        [Fact]
        public async void Should_Return_User_Payslips_Wages()
        {
            //Arrange
            var expectedValue = new List<UserPayslipWagesDTO>()
            {
                new UserPayslipWagesDTO ()
                {
                    Year = 1401,
                    Months = new List<Month>() { Month.Aban, Month.Azar, Month.Dey, Month.Bahman, Month.Esfand }
                },
                new UserPayslipWagesDTO ()
                {
                    Year = 1402,
                    Months = new List<Month>() { Month.Farvardin, Month.Ordibehesht}
                },
            };

            A.CallTo(() => _payslipService.GetUserWages(A<Guid>._)).Returns(expectedValue);

            //Act
            var response = await _payslipsController.GetUserPayslipWages();
            var result = (OkObjectResult)response;
            var wages = result.Value as List<UserPayslipWagesDTO>;

            response.Should().NotBeNull();
            response.Should().BeOfType<OkObjectResult>();
            result.StatusCode.Should().Be(200);

            wages!.Select(s => s.Year).Should().Equal(expectedValue.Select(s => s.Year));
            wages!.Select(s => s.Months).Should().HaveCountGreaterThan(0);
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
            var response = await _payslipsController.UploadPayslipFile(fakeDTO, 1402, 8);
            var result = (OkResult)response;

            //Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<OkResult>();
            result.StatusCode.Should().Be(200);
        }
    }
}
