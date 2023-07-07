using Lumberjack.API.Entities;
using Lumberjack.API.Metadata;

namespace Lumberjack.API.Services
{
    public interface ILumberjackRepository
    {
        Task<IEnumerable<Message>> GetMessagesAsync();
        Task<(IEnumerable<Message>, PaginationMetadata)> GetMessagesAsync(int? applicationId, string? searchQuery, int pageNumber, int pageSize);
        Task<Message?> GetMessageAsync(int messageId, bool includeSegments, bool includeFiles);
        Task<bool> MessageExistsAsync(int messageId);
        Task<bool> MessageBelongsToApplicationAsync(int messageId, int applicationId);
        Task<bool> IsMessageTypeSegmentAsync(int messageId);
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        
        Task<IEnumerable<Segment>> GetSegmentsForMessageAsync(int messageId);
        Task<Segment?> GetSegmentForMessageAsync(int messageId, int segmentId);
        Task AddSegmentForMessageAsync(int messageId, Segment segment);

        Task<IEnumerable<Entities.File>> GetFilesForMessageAsync(int messageId);
        Task<Entities.File?> GetFileForMessageAsync(int messageId, int fileId);
        Task AddFileForMessageAsync(int messageId, Entities.File file);

        
        Task<User?> GetUserAsync(string userName);
        Task<(User?, IEnumerable<Application>)> GetUserInfoForUsernameAsync(string userName);
        Task<User?> ValidateUserAsync(string userName, int applicationId);
        Task<UserApplicationMap?> GetUserApplicationMapAsync(int userId, int applicationId);

        Task<bool> SaveChangesAsync();
        
    }
}                      