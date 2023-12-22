using FluentAssertions;
using Payslip.API.Helpers;
using System.IO;
using Xunit;

namespace Payslip_Api.Sections.Payslips.Helpers
{
    public class PayslipExtractorHelperTest
    {
        private readonly ExcelHelpler _payslipExtractorHelper;

        public PayslipExtractorHelperTest()
        {
            _payslipExtractorHelper = new ExcelHelpler();
        }

        [Fact]
        public void Should_Extract_Payslip_From_File()
        {
            //Arrange
            var file = Directory.GetCurrentDirectory() + "\\Fixtures\\PayslipTest.xlsx";
            Stream stream = new FileStream(file, FileMode.Open);

            //Act
            var extractedPayslips = _payslipExtractorHelper.ExtractPayslips(stream);

            //Assert
            extractedPayslips.Should().NotBeNull();
            extractedPayslips.Should().HaveCountGreaterThan(0);
            extractedPayslips.Should().HaveCount(3);
        }
    }
}
