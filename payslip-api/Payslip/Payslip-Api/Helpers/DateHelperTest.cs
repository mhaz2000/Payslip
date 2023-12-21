using FluentAssertions;
using Payslip.Application.Base;
using Payslip.Application.Helpers;
using Payslip.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Payslip_Api.Helpers
{
    public class DateHelperTest
    {
        [Theory]
        [InlineData(1400, 5)]
        public void Should_return_year_and_parsed_month(int year, int month)
        {
            //Arrange
            int expectedYear = year;
            Month expectedMonth = Month.Mordad;

            //Act
            (int outputYear, Month outputMonth) = DateHelper.ValidateDate(year, month);

            //Assert
            outputYear.Should().Be(expectedYear);
            outputMonth.Should().Be(expectedMonth);
        }

        [Theory]
        [InlineData(1200, 4)]
        public void Should_throw_exception_when_year_is_not_valid(int year, int month)
        {
            //Act
            var act= ()=> DateHelper.ValidateDate(year, month);

            //Assert
            act.Should().Throw<ManagedException>().WithMessage("سال مالی صحیح نمی باشد.");
        }

        [Theory]
        [InlineData(1400, 15)]
        public void Should_throw_exception_when_month_is_not_valid(int year, int month)
        {
            //Act
            var act = () => DateHelper.ValidateDate(year, month);

            //Assert
            act.Should().Throw<ManagedException>().WithMessage("ماه صحیح نمی باشد.");
        }
    }
}
