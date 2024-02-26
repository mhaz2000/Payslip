using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Payslip.API.Base;
using Payslip.API.Controllers;
using Payslip.API.Helpers;
using Payslip.Application.DTOs;
using Payslip.Application.Helpers;
using Payslip.Application.Services;
using Payslip.Core.Enums;
using Payslip_Api.Fixtures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Payslip_Api.Sections.Payslips.Actions
{
    public class PayslipsControllerTest
    {
        private readonly IFileService _fileService;
        private readonly IPayslipService _payslipService;
        private readonly PayslipsController _payslipsController;
        private readonly IConfiguration _config;
        public PayslipsControllerTest()
        {
            _config = Constant.InitConfiguration();

            _fileService = A.Fake<IFileService>();
            _payslipService = A.Fake<IPayslipService>();
            var payslipHelper = A.Fake<IExcelHelpler>();

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

            _payslipsController = new PayslipsController(_payslipService, payslipHelper, _fileService);
            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext,
            };

            _payslipsController.ControllerContext = context;
        }

        #region remove payslip

        [Fact]
        public async void Should_Remove_Payslip()
        {
            //Arrange
            var id = Guid.NewGuid();

            //Act
            var respnose = await _payslipsController.RemovePayslip(id);
            var result = (OkResult)respnose;

            respnose.Should().NotBeNull();
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
        }

        #endregion

        #region get payslips

        [Fact]
        public async Task Should_Get_All_Uploaded_Payslips()
        {
            //Arrange
            var expectedValues = new List<PayslipDTO>()
            {
                new PayslipDTO()
                {
                    FileId = Guid.NewGuid(),
                    Month = Month.Aban.GetDescription(),
                    UploadedDate= DateTime.Now,
                    Year = 1402
                },
                new PayslipDTO()
                {
                    FileId = Guid.NewGuid(),
                    Month = Month.Azar.GetDescription(),
                    UploadedDate= DateTime.Now,
                    Year = 1402
                },
                new PayslipDTO()
                {
                    FileId = Guid.NewGuid(),
                    Month = Month.Dey.GetDescription(),
                    UploadedDate= DateTime.Now,
                    Year = 1402
                }
            };

            A.CallTo(() => _payslipService.GetPayslips(A<int>._)).Returns((expectedValues, 3));

            //Act
            var response = await _payslipsController.GetPayslips(5);
            var result = (OkObjectResult)response;
            var responseModel = result.Value as ResponseModel;

            result.StatusCode.Should().Be(200);
            responseModel.Should().NotBeNull();
            responseModel.Total.Should().Be(3);

            responseModel.Data.Should().NotBeNull();
            responseModel.Data.Should().BeEquivalentTo(expectedValues);
        }

        #endregion

        #region get user payslips wages
        [Fact]
        public async void Should_Return_User_Payslips_Wages()
        {
            //Arrange
            var expectedValue = new List<UserPayslipWagesDTO>()
            {
                new UserPayslipWagesDTO ()
                {
                    Year = 1401,
                    Months = new List<int>() { Month.Aban.GetHashCode(), Month.Azar.GetHashCode(), Month.Dey.GetHashCode(), Month.Bahman.GetHashCode(), Month.Esfand.GetHashCode() }
                },
                new UserPayslipWagesDTO ()
                {
                    Year = 1402,
                    Months = new List<int>() { Month.Farvardin.GetHashCode(), Month.Ordibehesht.GetHashCode() }
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
        #endregion

        #region payslip upload file

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
            A.CallTo(() => _fileService.StoreFile(A<Stream>._, A<string>._)).MustHaveHappenedOnceExactly();

            response.Should().NotBeNull();
            response.Should().BeOfType<OkResult>();
            result.StatusCode.Should().Be(200);
        }

        #endregion

        #region Get User Payslip

        [Fact]
        public async Task Should_return_user_payslip()
        {
            var response = await _payslipsController.GetUserPayslip(1,1402);
            var result = (OkObjectResult)response;

            response.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().NotBeNull();
            result.Value.Should().BeAssignableTo<UserPayslipDTO>();
        }

        #endregion
    }
}
