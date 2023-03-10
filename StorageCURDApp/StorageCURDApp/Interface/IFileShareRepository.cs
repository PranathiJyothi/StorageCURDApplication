namespace StorageCURDApp.Interface
{
    public interface IFileShareRepository
    {
        Task<bool> UploadFile(IFormFile file);
        Task<byte[]> DownloadFile(string fileName);
        Task<bool> DeleteFileAsync(string fileName);
    }
}
