using StorageCURDApp.Model;

namespace StorageCURDApp.Interface
{
    public interface IQueueRepository
    {
        Task<string> AddMessageAsync(QueueMessage message);
        Task<QueueMessage> GetMessageAsync();
        Task<QueueMessage> DequeueMessageAsync();
        Task<string> UpdateMessageAsync(QueueMessage message);
        Task<string> DeleteMessagesAsync();
    }
}
