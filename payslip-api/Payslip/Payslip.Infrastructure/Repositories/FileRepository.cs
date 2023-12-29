using Payslip.Core.Entities;
using Payslip.Core.Repositories;
using Payslip.Infrastructure.Data;
using Payslip.Infrastructure.Repositories.Base;

namespace Payslip.Infrastructure.Repositories
{
    public class FileRepository : Repository<FileModel>, IFileRepository
    {
        public FileRepository(DataContext context) : base(context)
        {
        }
    }
}
