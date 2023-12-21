using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Payslip.Infrastructure.Data;
using Payslip.Infrastructure.Repositories;
using Payslip_Api.Fixtures;
using System.Threading;
using Xunit;

namespace Payslip_Api.Sections.Authentications.Repositories
{
    public class RoleRepositoryTest : RepositoryTest
    {
        private readonly RoleRepository _roleRepository;
        public RoleRepositoryTest()
        {
            //Arrange
            _roleRepository = new RoleRepository(_dataContext);
        }

        [Fact]
        public async void Should_Return_Roles_of_User()
        {
            //Act
            var user = await _dataContext.Users.LastOrDefaultAsync();
            var roles = _roleRepository.GetUserRoles(user!);

            //Assert
            roles.Should().NotBeNull();
            roles.Should().HaveCountGreaterThan(0);
        }
    }
}
