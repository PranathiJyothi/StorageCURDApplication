using Azure.Storage.Queues;
using Newtonsoft.Json;
using StorageCURDApp.Interface;
using StorageCURDApp.Model;

namespace StorageCURDApp.Repository
{
    public class QueueRepository : IQueueRepository
    {
        private readonly QueueClient queueClient;
        public QueueRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("StorageConnectionString");
            string queueName = configuration.GetValue<string>("QueueStorage:QueueName");
            queueClient = new QueueClient(connectionString, queueName);

        }

        public async Task<string> AddMessageAsync(QueueMessage message)
        {
            try
            {
                var messageBody = JsonConvert.SerializeObject(message);
                await queueClient.CreateIfNotExistsAsync();
                await queueClient.SendMessageAsync(messageBody);
                return "Added the message to the queue";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Message is not added to the queue";
            }
        }

        public async Task<string> DeleteMessagesAsync()
        {
            await queueClient.ClearMessagesAsync();
            return "Deleted all the messages in the queue";
        }

        public async Task<QueueMessage> DequeueMessageAsync()
        {
            QueueMessage message = null;
            var receivedMessage = await queueClient.ReceiveMessageAsync();

            if (receivedMessage != null)
            {
                message = JsonConvert.DeserializeObject<QueueMessage>(receivedMessage.Value.MessageText);
                await queueClient.DeleteMessageAsync(receivedMessage.Value.MessageId, receivedMessage.Value.PopReceipt);
            }

            return message;
        }

        public async Task<QueueMessage> GetMessageAsync()
        {
            var response = await queueClient.PeekMessageAsync();
            if (response == null || response.Value == null || response.Value.MessageText == null)
            {
                return null;
            }
            var jsonMessage = response.Value.Body.ToString();
            var message = JsonConvert.DeserializeObject<QueueMessage>(jsonMessage);
            return message;
        }

        public async Task<string> UpdateMessageAsync(QueueMessage message)
        {
            var receivedMessage = await queueClient.ReceiveMessageAsync();
            if (receivedMessage?.Value != null)
            {
                var updatedMessage = JsonConvert.DeserializeObject<QueueMessage>(receivedMessage.Value.MessageText);
                updatedMessage.MessageId = message.MessageId;
                updatedMessage.Content = message.Content;
                var messageBody = JsonConvert.SerializeObject(updatedMessage);
                await queueClient.UpdateMessageAsync(receivedMessage.Value.MessageId, receivedMessage.Value.PopReceipt, messageBody, TimeSpan.Zero);
            }
            return "Updated the message to the queue";
        }
    }
}
