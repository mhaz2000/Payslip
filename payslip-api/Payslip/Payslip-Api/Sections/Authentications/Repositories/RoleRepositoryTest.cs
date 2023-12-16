using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Payslip.Infrastructure.Data;
using Payslip.Infrastructure.Repositories;
using Payslip_Api.Fixtures;
using Xunit;

namespace Payslip_Api.Sections.Authentications.Repositories
{
    public class RoleRepositoryTest
    {
        private readonly DataContext _dataContext;
        private readonly RoleRepository _roleRepository;
        public RoleRepositoryTest()
        {
            //Arrange
            _dataContext = ContextFixture.GetDatabaseContext().Result;
            _roleRepository = new RoleRepository(_dataContext);
        }

        [Fact]
        public async void Should_Return_Roles_of_User()
        {
            //Act
            var user = await _dataContext.Users.FirstOrDefaultAsync();
            var roles = _roleRepository.GetUserRoles(user!);

            //Assert
            roles.Should().NotBeNull();
            roles.Should().HaveCountGreaterThan(0);
        }
    }
}
