namespace Lumberjack.API.Services
{
    public interface IFileService
    {
        Task<(string, string)> StoreFile(IFormFile formFile);
    }
}
