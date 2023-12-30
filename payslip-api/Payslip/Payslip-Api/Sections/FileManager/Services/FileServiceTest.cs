using FakeItEasy;
using FluentAssertions;
using Payslip.Application.Base;
using Payslip.Application.Services;
using Payslip.Core.Entities;
using Payslip.Core.Repositories.Base;
using System;
using System.IO;
using Xunit;

namespace Payslip_Api.Sections.FileManager.Services
{
    public class FileServiceTest
    {
        private readonly FileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        public FileServiceTest()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _fileService = new FileService(_unitOfWork);
        }

        #region store file
        [Fact]
        public async void Should_Store_File()
        {
            //Arrange
            var file = Directory.GetCurrentDirectory() + "\\Fixtures\\FileTest.xlsx";
            Stream stream = new FileStream(file, FileMode.Open);

            //Act
            await _fileService.StoreFile(stream, "FileTest.xlsx");

            A.CallTo(() => _unitOfWork.FileRepository.AddAsync(A<FileModel>._)).MustHaveHappened();

        }

        #endregion

        #region Get file

        [Fact]
        public async void Should_Raise_Error_When_File_Does_Not_Exist()
        {
            //Arrange
            var id = Guid.NewGuid();
            FileModel file = null;
            //Act
            A.CallTo(() => _unitOfWork.FileRepository.GetByIdAsync(id))!.Returns(file);

            //Assert
            await _fileService.Invoking(c=> c.GetFile(id)).Should().ThrowAsync<ManagedException>().WithMessage("فایل مورد نظر یافت نشد.");
        }

        #endregion
    }
}
