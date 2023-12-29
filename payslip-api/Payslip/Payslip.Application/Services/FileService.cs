using Payslip.Application.Base;
using Payslip.Core.Entities;
using Payslip.Core.Repositories.Base;

namespace Payslip.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FileStream> GetFile(Guid id)
        {
            var fileModel = await _unitOfWork.FileRepository.GetByIdAsync(id);
            if (fileModel is null)
                throw new ManagedException("فایل مورد نظر یافت نشد.");

            var path = Directory.GetCurrentDirectory() + "\\FileManager";
            var filePath = Path.Combine(path, $"{id}.dat");

            if (!File.Exists(filePath))
                throw new ManagedException("فایل مورد نظر یافت نشد.");

            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public async Task<Guid> StoreFile(Stream file, string fileName)
        {
            var path = Directory.GetCurrentDirectory() + "\\FileManager";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fileId = Guid.NewGuid();
            var dir = Path.Combine(path, $"{fileId}.dat");

            using (var fileStream = new FileStream(dir, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
            {
                await file.CopyToAsync(fileStream);
            }

            FileModel fileModel = new FileModel(fileName, fileId);

            await _unitOfWork.FileRepository.AddAsync(fileModel);

            return fileId;
        }
    }
}
