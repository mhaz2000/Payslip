using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Payslip.Infrastructure.Repositories;
using Payslip_Api.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Payslip_Api.Sections.Payslips.Repositories
{
    public class PayslipRepositoryTest : RepositoryTest
    {
        private readonly PayslipRepository _payslipRepository;
        public PayslipRepositoryTest()
        {
            _payslipRepository = new PayslipRepository(_dataContext);
        }

        [Fact]
        public async void Should_Get_User_Payslips()
        {
            var expected = await _dataContext.UserPayslips.FirstOrDefaultAsync();
            var payslips = _payslipRepository.GetUserPayslips(expected.User.Id);

            payslips.Should().NotBeNull();
            payslips.Should().HaveCount(1);
            payslips.FirstOrDefault().Should().BeEquivalentTo(expected);
        }
    }
}
