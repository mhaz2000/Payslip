using Microsoft.AspNetCore.Http;

namespace Payslip.Application.Services
{
    public interface IFileService
    {
        Task<Guid> StoreFile(Stream file, string fileName);

        Task<(FileStream stream, string filename)> GetFile(Guid id);

        Task RemoveFile(Guid id);
    }
}
