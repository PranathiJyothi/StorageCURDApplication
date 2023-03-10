using Azure;
using Azure.Data.Tables;
using StorageCURDApp.Interface;

namespace StorageCURDApp.Repository
{
    public class TableRepository : ITableRepository
    {
        private readonly IConfiguration _configuration;

        public TableRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private async Task<TableClient> GetTableClient()
        {
            var serviceClient = new TableServiceClient(_configuration["ConnectionStrings:StorageConnectionString"]);
            var tableClient = serviceClient.GetTableClient(_configuration["TableStorage:TableName"]);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }
        public async Task<Model.TableEntity> GetEntityAsync(string category, string id)
        {
            var tableClient = await GetTableClient();
            
            return await tableClient.GetEntityAsync <Model.TableEntity>(category, id);
            
        }
        public async Task<Model.TableEntity> AddEntityAsync(Model.TableEntity entity)
        {
            var tableClient = await GetTableClient();
            await tableClient.AddEntityAsync(entity);
            return entity;
        }
        public async Task<Model.TableEntity> UpsertEntityAsync(Model.TableEntity entity)
        {
            var tableClient = await GetTableClient();
            await tableClient.UpsertEntityAsync(entity);
            return entity;
        }
        public async Task DeleteEntityAsync(string category, string id)
        {
            var tableClient = await GetTableClient();
            await tableClient.DeleteEntityAsync(category, id);
        }

        
    }
}
