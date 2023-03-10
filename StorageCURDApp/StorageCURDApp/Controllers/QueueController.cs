using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageCURDApp.Interface;
using StorageCURDApp.Model;

namespace StorageCURDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IQueueRepository _queueRepository;

        public QueueController(IQueueRepository queueRepository)
        {
            this._queueRepository = queueRepository;
        }

        [HttpPost("AddMessage")]
        public async Task<string> AddMessage(QueueMessage message)
        {
            var result = await _queueRepository.AddMessageAsync(message);
            return result;
        }

        [HttpGet("GetMessage")]
        public async Task<IActionResult> GetMessage()
        {
            var message = await _queueRepository.GetMessageAsync();
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        [HttpGet("DequeueMessage")]
        public async Task<QueueMessage> DequeueMessage()
        {
            return await _queueRepository.DequeueMessageAsync();
        }

        [HttpPut("UpdateMessage")]
        public async Task<string> UpdateMessage(QueueMessage message)
        {
            var result = await _queueRepository.UpdateMessageAsync(message);
            return result;
        }

        [HttpDelete("DeleteMessages")]
        public async Task<string> DeleteMessage()
        {
            var result = await _queueRepository.DeleteMessagesAsync();
            return result;
        }

        
    }
}
