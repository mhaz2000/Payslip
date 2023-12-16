using Bogus;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Payslip.Core.Entities;
using Payslip.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payslip_Api.Fixtures
{
    internal class ContextFixture
    {
        private static Random random = new Random();
        public static async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var fakeDbContext = new DataContext(options);
            fakeDbContext.Database.EnsureCreated();

            await FakeRoleIdentity(fakeDbContext);

            fakeDbContext.SaveChanges();

            return fakeDbContext;
        }

        public static async Task FakeRoleIdentity(DataContext fakeDbContext)
        {
            var generatedRoles = GenerateIdentityRoleData();
            await fakeDbContext.Roles.AddRangeAsync(generatedRoles);
            fakeDbContext.SaveChanges();

            var generatedUsers = GenerateUserData(10);

            var userStore = new UserStore<User, IdentityRole<Guid>, DataContext, Guid>(fakeDbContext);
            var userManager = new UserManager<User>(userStore, null, new PasswordHasher<User>(), null, null, null, null, null, null);


            foreach (var newUser in generatedUsers)
            {
                await userManager.CreateAsync(newUser, "123456");
                await userManager.AddToRoleAsync(newUser, generatedRoles[random.Next(0, 2)].NormalizedName);
            }

        }

        private static List<IdentityRole<Guid>> GenerateIdentityRoleData()
        {
            return new List<IdentityRole<Guid>>()
            {
                new IdentityRole<Guid>()
                {
                    Name = "Admin",
                    Id = Guid.NewGuid(),
                    NormalizedName = "admin"
                },
                new IdentityRole<Guid>()
                {
                    Name = "User",
                    Id = Guid.NewGuid(),
                    NormalizedName = "user"
                }
            };

        }

        private static List<User> GenerateUserData(int count)
        {
            var faker = new Faker<User>()
                .RuleFor(c => c.UserName, f => f.Name.FindName().Replace(" ", ""))
                .RuleFor(c => c.FirstName, f => f.Person.FirstName)
                .RuleFor(c => c.LastName, f => f.Person.LastName)
                .RuleFor(c => c.NationalCode, f => random.NextInt64(1111111111, 9999999999).ToString())
                .RuleFor(c => c.PersonnelCode, f => random.NextInt64(111111, 999999).ToString())
                .RuleFor(c => c.Id, f => Guid.NewGuid());

            return faker.Generate(count);
        }


    }
}
