using Microsoft.AspNetCore.Http;

namespace Payslip.Application.Services
{
    public interface IFileService
    {
        Task<Guid> StoreFile(Stream file, string fileName);

        Task<FileStream> GetFile(Guid id);
    }
}
