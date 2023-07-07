namespace Lumberjack.API.Utilities
{
    public static class Extensions
    {
        public static string GenerateFileSize(this long bytes)
        {
            const int kilobyte = 1024;
            const int megabyte = kilobyte * 1024;

            if (bytes >= megabyte && bytes % megabyte == 0)
            {
                int sizeInMB = (int)(bytes / megabyte);
                return $"{sizeInMB} MB";
            }
            else if (bytes >= kilobyte && bytes % kilobyte == 0)
            {
                int sizeInKB = (int)(bytes / kilobyte);
                return $"{sizeInKB} KB";
            }
            else
            {
                return $"{bytes} bytes";
            }
        }
    }
}
