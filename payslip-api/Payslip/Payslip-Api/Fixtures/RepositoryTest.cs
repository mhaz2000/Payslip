using Bogus;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Payslip.Core.Entities;
using Payslip.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payslip_Api.Fixtures
{
    public class RepositoryTest : IDisposable
    {
        protected DataContext _dataContext { get; private set; }

        protected RepositoryTest()
        {
            InitializeDataContext();
            FakePayslips(3);
            FakeRoleIdentity().Wait();
        }

        private static Random random = new Random();

        private void InitializeDataContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase" + Guid.NewGuid().ToString())
                .Options;

            _dataContext = new DataContext(options);
        }

        private async Task FakeRoleIdentity()
        {
            var generatedRoles = GenerateIdentityRoleData();
            await _dataContext.Roles.AddRangeAsync(generatedRoles);
            await _dataContext.SaveChangesAsync();

            var generatedUsers = GenerateUserData(10);

            var userStore = new UserStore<User, IdentityRole<Guid>, DataContext, Guid>(_dataContext);
            var userManager = new UserManager<User>(userStore, null, new PasswordHasher<User>(), null, null, null, null, null, null);

            foreach (var newUser in generatedUsers)
            {
                await userManager.CreateAsync(newUser, "123456");
                await userManager.AddToRoleAsync(newUser, generatedRoles[random.Next(0, 2)].NormalizedName);
            }
        }

        private void FakePayslips(int count)
        {
            var faker = new Faker<UserPayslip>()
                .RuleFor(c => c.FirstName, f => f.Person.FirstName)
                .RuleFor(c => c.CardNumber, f => random.NextInt64(111111, 999999).ToString())
                .RuleFor(c => c.LastName, f => f.Person.LastName);

            var payslips = faker.Generate(count);
            payslips.ForEach(payslip =>
            {
                payslip.User = new User()
                {
                    FirstName = payslip.FirstName,
                    LastName = payslip.FirstName,
                    CardNumber = payslip.CardNumber,
                    NationalCode = random.NextInt64(1111111111, 9999999999).ToString()
                };
            });

            _dataContext.UserPayslips.AddRange(payslips);
            _dataContext.SaveChanges();
        }

        private List<IdentityRole<Guid>> GenerateIdentityRoleData()
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

        private List<User> GenerateUserData(int count)
        {
            var faker = new Faker<User>()
                .RuleFor(c => c.UserName, f => f.Name.FindName().Replace(" ", ""))
                .RuleFor(c => c.FirstName, f => f.Person.FirstName)
                .RuleFor(c => c.LastName, f => f.Person.LastName)
                .RuleFor(c => c.NationalCode, f => random.NextInt64(1111111111, 9999999999).ToString())
                .RuleFor(c => c.CardNumber, f => random.NextInt64(111111, 999999).ToString())
                .RuleFor(c => c.Id, f => Guid.NewGuid());

            return faker.Generate(count);
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }
    }
}
