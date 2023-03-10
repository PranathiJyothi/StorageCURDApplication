using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageCURDApp.Interface;
using StorageCURDApp.Model;
using System.IO;

namespace StorageCURDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileShareController : ControllerBase
    {
        private readonly IFileShareRepository _fileshareRepository;

        public FileShareController(IFileShareRepository repository)
        {
            _fileshareRepository = repository;
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] FileShareModel file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _fileshareRepository.UploadFile(file.FileDetail);

            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var file = await _fileshareRepository.DownloadFile(fileName);

            if (file == null)
            {
                return NotFound();
            }

            return File(file, "application/octet-stream", fileName);
        }

       
        [HttpDelete("DeleteFile")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            var result = await _fileshareRepository.DeleteFileAsync(fileName);

            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
