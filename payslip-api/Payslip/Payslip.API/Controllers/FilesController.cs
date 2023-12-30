using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Payslip.API.Base;
using Payslip.Application.Services;

namespace Payslip.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ApiControllerBase
    {
        private readonly IFileService _fileService;
        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("{id}")]
        public async Task<FileResult> Get(Guid id)
        {
            var file = await _fileService.GetFile(id);
            var fileStream = file.stream;
            var fileName = file.filename;

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var cdStr = $"inline; filename=\"{fileName}\"";

            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Disposition");
            //Response.Headers.Add("Content-Disposition", cdStr);
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            return File(fileStream, contentType, fileName);
        }
    }
}
