using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Payslip.API.Helpers;
using Payslip.Core.Entities;
using Payslip.Infrastructure.Data;

namespace Payslip.API.Extensions
{
    public static class HostExtensions
    {
        private static readonly DbContextFactory MyDbContextFactory = new DbContextFactory();

        private static IUserStore<User> _userStore;
        private static UserManager<User> _userManager;

        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    var dataContext = MyDbContextFactory.CreateDbContext(new[] { string.Empty });

                    _userStore = new UserStore<User, IdentityRole<Guid>, DataContext, Guid>(dataContext);
                    _userManager = new UserManager<User>(_userStore, null, new PasswordHasher<User>(), null, null, null, null, null, null);
                    dataContext.Database.Migrate();

                    CreateRolesSeed(dataContext);
                    CreateAdminSeed(dataContext, _userManager);
                    UsersSeed(dataContext, _userManager, services.GetRequiredService<IExcelHelpler>());

                    logger.LogInformation("Migrating database");
                }
                catch (Exception ex)
                {
                    logger.LogError("An error has been occured\n" + ex);

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
            }

            return host;
        }

        private static void UsersSeed(DataContext dataContext, UserManager<User> userManager, IExcelHelpler excelHelpler)
        {
            FileStream file = new FileStream(Directory.GetCurrentDirectory() + "\\StaticFiles\\Users.xlsx", FileMode.Open);
            var users = excelHelpler.ExtractUsers(file);

            string username = "admin";

            if (!dataContext.Users.Any(u => u.UserName != username))
            {
                foreach (var user in users)
                {
                    user.FirstName = user.FirstName.Replace('ي', 'ی').Replace("ك", "ک");
                    user.LastName = user.LastName.Replace('ي', 'ی').Replace("ك", "ک");
                    var newUser = new User(user.NationalCode, user.LastName, user.FirstName, user.NationalCode, user.CardNumber)
                    {
                        NormalizedUserName = user.NationalCode,
                    };

                    userManager.CreateAsync(newUser, user.NationalCode);
                }
                dataContext.SaveChanges();
            }
        }

        private static void CreateAdminSeed(DataContext context, UserManager<User> userManager)
        {
            string username = "admin";

            if (!context.Users.Any(u => u.UserName == username))
            {
                var newUser = new User("admin", "کاربر", "ادمین", string.Empty, string.Empty)
                {
                    NormalizedUserName = "admin",
                };

                var done = userManager.CreateAsync(newUser, "123456");

                if (done.Result.Succeeded)
                    userManager.AddToRoleAsync(newUser, "Admin");

                context.SaveChanges();
            }
        }

        private static void CreateRolesSeed(DataContext context)
        {
            var role = context.Roles.AnyAsync(c => c.Name == "Admin");

            if (!role.Result)
            {
                var newAdminRole = new IdentityRole<Guid>()
                {
                    Name = "Admin",
                    Id = Guid.NewGuid(),
                    NormalizedName = "admin"
                };

                context.Roles.Add(newAdminRole);

                var newUserRole = new IdentityRole<Guid>()
                {
                    Name = "User",
                    NormalizedName = "user",
                    Id = Guid.NewGuid(),
                };
                context.Roles.Add(newUserRole);

                context.SaveChanges();
            }
        }
    }
}
