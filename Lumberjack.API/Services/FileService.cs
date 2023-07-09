using Lumberjack.API.Entities;
using Microsoft.AspNetCore.Http;

namespace Lumberjack.API.Services
{
    public class FileService : IFileService
    {
        private readonly string _internalFilePath;

        public FileService(IConfiguration configuration)
        {
            _internalFilePath = configuration["FileSettings:InternalFilePath"];
        }

        public async Task<(string, string)> StoreFile(IFormFile formFile)
        {
            try 
            {
                var folderName = DateTime.Now.ToString("yyyyMMdd");
                var logDirectory = Path.Combine(_internalFilePath, folderName);

                if (!Directory.Exists(logDirectory))
                { 
                    Directory.CreateDirectory(logDirectory);
                }

                var dateTimeString = DateTime.Now.ToString("yyyyMMdd'_T'HHmmss");

                var logFileName = $"{formFile.FileName}.{dateTimeString}";

                var logFilePath = Path.Combine(logDirectory, logFileName);                

                using (var stream = new FileStream(logFilePath, FileMode.Create))
                {
                    formFile.CopyToAsync(stream).Wait();
                }

                return (logFileName, logDirectory);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Exception while storing File with name {formFile.FileName}", ex);
            }
        }
    }
}