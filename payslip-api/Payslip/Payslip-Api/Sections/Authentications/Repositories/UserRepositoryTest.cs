using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Payslip.Infrastructure.Data;
using Payslip.Infrastructure.Repositories;
using Payslip_Api.Fixtures;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Payslip_Api.Sections.Authentications.Repositories
{
    public class UserRepositoryTest : RepositoryTest
    {
        private readonly UserRepository _userRepository; 

        public UserRepositoryTest()
        {
            _userRepository = new UserRepository(_dataContext);
        }

        [Fact]
        public async Task Should_Return_User_Based_On_Card_Number()
        {
            //Arrange
            var expectedUser = await _dataContext.Users.FirstOrDefaultAsync();

            //Act
            var user = _userRepository.GetUserByCardNumber(expectedUser.CardNumber);

            //Assert
            user.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task Should_Return_Null_When_Card_Number_Is_Not_Valid()
        {
            //Arrange
            var cardNumber = "00000000";

            //Act
            var user = _userRepository.GetUserByCardNumber(cardNumber);

            //Assert
            user.Should().BeNull();
        }
    }
}
