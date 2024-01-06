using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Payslip.Infrastructure.Data;
using Newtonsoft.Json;
using Payslip.Application.Base;

namespace Payslip.API.Extensions
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            AppSettingsModel appSettingsModel = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CONFIG")) ? configuration.Get<AppSettingsModel>() :
                JsonConvert.DeserializeObject<AppSettingsModel>(Environment.GetEnvironmentVariable("CONFIG")!)!;

            var connectionString = appSettingsModel.ConnectionStrings.Main;

            optionsBuilder.UseSqlServer(connectionString);

            return new DataContext(optionsBuilder.Options);
        }
    }
}
