using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageCURDApp.Interface;
using StorageCURDApp.Model;

namespace StorageCURDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly IBlobRepository _repository;

        public BlobController(IBlobRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("Retrive")]
        public async Task<IActionResult> GetAsync()
        {
            var blobs = await _repository.GetFileAsync();
            return Ok(blobs);
        }

        [HttpGet("RetriveByName")]
        public async Task<ActionResult<Blob>> GetAsync(string blobfileName)
        {
            var blob = await _repository.GetFileAsync(blobfileName);
            if (blob == null)
            {
                return NotFound();
            }
            return blob;
        }

        [HttpPost("Upload")]
        public async Task<ActionResult<Blob>> AddAsync(IFormFile file, string blobfileName)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest();
            }
            var blob = await _repository.AddFileAsync(file.OpenReadStream(), blobfileName);
            return blob;
        }

       

        [HttpDelete("Delete")]
        public async Task<string> DeleteAsync(string blobfileName)
        {
            var result = await _repository.DeleteFileAsync(blobfileName);
            return result;
        }
    }
}
