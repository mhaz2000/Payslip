using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Payslip.Core.Entities;
using Payslip.Infrastructure.Extensions;
using System.IO;

namespace Payslip.Infrastructure.Data
{
    public class DataContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<FileModel> FileModels { get; set; }
        public virtual DbSet<UserPayslip> UserPayslips { get; set; }
        public virtual DbSet<IdentityRole<Guid>> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserPayslip>()
                .Property(c => c.SalaryAndBenefits)
                .HasColumnType("nvarchar(max)")
                .HasConversion(
                    data => data.ToJson(),
                    data => JsonConvert.DeserializeObject<IDictionary<int, string>>(data)!);

            builder.Entity<UserPayslip>()
                .Property(c => c.SalaryAndBenefitsAmount)
                .HasColumnType("nvarchar(max)")
                .HasConversion(
                    data => data.ToJson(),
                    data => JsonConvert.DeserializeObject<IDictionary<int, string>>(data)!);

            builder.Entity<UserPayslip>()
                .Property(c => c.Deductions)
                .HasColumnType("nvarchar(max)")
                .HasConversion(
                    data => data.ToJson(),
                    data => JsonConvert.DeserializeObject<IDictionary<int, string>>(data)!);

            builder.Entity<UserPayslip>()
                .Property(c => c.DeductionsAmount)
                .HasColumnType("nvarchar(max)")
                .HasConversion(
                    data => data.ToJson(),
                    data => JsonConvert.DeserializeObject<IDictionary<int, string>>(data)!);

            builder.Entity<UserPayslip>()
                .Property(c => c.Durations)
                .HasColumnType("nvarchar(max)")
                .HasConversion(
                    data => data.ToJson(),
                    data => JsonConvert.DeserializeObject<IDictionary<int, string>>(data)!);
        }
    }
}
