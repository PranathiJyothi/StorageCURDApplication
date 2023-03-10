using StorageCURDApp.Model;

namespace StorageCURDApp.Interface
{
    public interface IBlobRepository
    {
        Task<Blob> AddFileAsync(Stream stream, string blobfileName);
        Task<IEnumerable<Blob>> GetFileAsync();
        Task<Blob> GetFileAsync(string blobfileName);
        Task<string> DeleteFileAsync(string blobfileName);
    }
}
