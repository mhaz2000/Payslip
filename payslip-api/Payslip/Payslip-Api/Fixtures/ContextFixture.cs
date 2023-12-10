using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Payslip.Core.Entities;
using Payslip.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Payslip_Api.Fixtures
{
    internal class ContextFixture
    {
        public static async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDb").Options;

            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();

            Random random = new Random();

            if (!await databaseContext.Users.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    await databaseContext.Users
                        .AddAsync(new User(Constant.LastNames[random.Next(0, Constant.LastNames.Count)], Constant.FirstNames[random.Next(0, Constant.FirstNames.Count)],
                        random.NextInt64(1111111111, 9999999999).ToString(), random.Next(111111,999999).ToString()));
                }

                var user = new User("محمد", "حبیب اله زاده", "1990919677", "110313");
                var _passwordHasher = new PasswordHasher<User>();
                user.PasswordHash = _passwordHasher.HashPassword(user, "123");

                await databaseContext.Users.AddAsync(user);

                await databaseContext.SaveChangesAsync();
            }

            return databaseContext;
        }
    }
}
