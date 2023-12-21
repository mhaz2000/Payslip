using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Payslip.Core.Entities;
using Payslip.Infrastructure.Extensions;

namespace Payslip.Infrastructure.Data
{
    public class DataContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPayslip> UserPayslips { get; set; }
        public virtual DbSet<IdentityRole<Guid>> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserPayslip>()
                .Property(c => c.SalaryAndBenefits)
                .HasColumnType("json")
                .HasConversion(
                    data => data.ToJson(),
                    data => JsonConvert.DeserializeObject<IDictionary<int, string>>(data)!);

            builder.Entity<UserPayslip>()
                .Property(c => c.SalaryAndBenefitsAmount)
                .HasColumnType("json")
                .HasConversion(
                    data => data.ToJson(),
                    data => JsonConvert.DeserializeObject<IDictionary<int, string>>(data)!);

            builder.Entity<UserPayslip>()
                .Property(c => c.Deductions)
                .HasColumnType("json")
                .HasConversion(
                    data => data.ToJson(),
                    data => JsonConvert.DeserializeObject<IDictionary<int, string>>(data)!);

            builder.Entity<UserPayslip>()
                .Property(c => c.DeductionsAmount)
                .HasColumnType("json")
                .HasConversion(
                    data => data.ToJson(),
                    data => JsonConvert.DeserializeObject<IDictionary<int, string>>(data)!);

            builder.Entity<UserPayslip>()
                .Property(c => c.Durations)
                .HasColumnType("json")
                .HasConversion(
                    data => data.ToJson(),
                    data => JsonConvert.DeserializeObject<IDictionary<int, string>>(data)!);
        }
    }
}
