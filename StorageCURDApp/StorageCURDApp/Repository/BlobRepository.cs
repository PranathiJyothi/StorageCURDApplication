using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using StorageCURDApp.Model;
using StorageCURDApp.Interface;

namespace StorageCURDApp.Repository
{
    public class BlobRepository : IBlobRepository
    {
        private readonly CloudBlobContainer _container;

        public BlobRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("StorageConnectionString");
            var containerName = configuration["BlobStorage:BlobName"];

            var blobClient = CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
        }

        public async Task<IEnumerable<Blob>> GetFileAsync()
        {
            var blobs = await _container.ListBlobsSegmentedAsync(null);
            return blobs.Results.Select(b => new Blob { BlobName = b.Uri.Segments.Last(), BlobUrl = b.Uri.AbsoluteUri });
        }
        //Get the container
        public async Task<Blob> GetFileAsync(string blobfileName)
        {
            var blob = _container.GetBlockBlobReference(blobfileName);
            if (await blob.ExistsAsync())
            {
                return new Blob
                {
                    BlobName = blobfileName,
                    BlobUrl = blob.Uri.AbsoluteUri
                };
            }
            return null;
        }

        //Adding the container
        public async Task<Blob> AddFileAsync(Stream stream, string blobfileName)
        {
            var blob = _container.GetBlockBlobReference(blobfileName);
            await blob.UploadFromStreamAsync(stream);
            return new Blob
            {
                BlobName = blobfileName,
                BlobUrl = blob.Uri.AbsoluteUri
            };
        }
        //deleting the container
        public async Task<string> DeleteFileAsync(string blobfileName)
        {
            var blob = _container.GetBlockBlobReference(blobfileName);
            await blob.DeleteAsync();
            return $"Deleted the blob file named {blobfileName}";
        }
    }
}
