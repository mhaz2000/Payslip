using Microsoft.EntityFrameworkCore;
using Payslip.Core.Entities;

namespace Payslip.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
