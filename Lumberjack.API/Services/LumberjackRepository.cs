using Lumberjack.API.Entities;
using Lumberjack.API.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Lumberjack.API.Services
{
    public class LumberjackRepository : ILumberjackRepository
    {
        private readonly LumberjackDBContext _context;

        public LumberjackRepository(LumberjackDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Messages Repository Methods

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _context.Messages.OrderByDescending(m => m.Id).ToListAsync();
        }

        public async Task<(IEnumerable<Message>, PaginationMetadata)> GetMessagesAsync(int? applicationId, string? searchQuery, 
                                                                    int pageNumber, int pageSize)
        {
            // This is done for deferred execution
            // IQueryable creates an expression tree
            // Execution is deferred until the query is iterated over: foreach, ToList(), ToArray(), ToDictionary()

            var collection = _context.Messages as IQueryable<Message>;

            if (applicationId != null)
            { 
                collection = collection.Where(c => c.ApplicationId == applicationId);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            { 
                searchQuery = searchQuery.Trim();
                collection = collection.Where(c => //c.ApplicationId.Contains(searchQuery) ||
                                                    (c.MessageType != null && c.MessageType.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderByDescending(c => c.Id)
                                    .Skip(pageSize * (pageNumber - 1))
                                    .Take(pageSize)
                                    .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }

        public async Task<Message?> GetMessageAsync(int messageId, bool includeSegments, bool includeFiles)
        {
            var message = _context.Messages.Where(m => m.Id == messageId);

            if (includeSegments)
            {
                message = message.Include(m => m.Segments);
            }
            if (includeFiles)
            {
                message = message.Include(m => m.Files);
            }

            return await message.FirstOrDefaultAsync();
        }

        public async Task<bool> MessageExistsAsync(int messageId)
        {
            return await _context.Messages.AnyAsync(m => m.Id == messageId);
        }

        public async Task<bool> MessageBelongsToApplicationAsync(int messageId, int applicationId)
        {
            return await _context.Messages.AnyAsync(m => m.Id == messageId 
                                                    && m.ApplicationId == applicationId);
        }

        public async Task<bool> IsMessageTypeSegmentAsync(int messageId)
        {
            return await _context.Messages.AnyAsync(m => m.Id == messageId
                                                    && m.MessageType.Equals("Segment", StringComparison.InvariantCultureIgnoreCase));
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {   
            _context.Messages.Remove(message);
        }

        #endregion

        #region Segments Repository Methods
        public async Task<IEnumerable<Segment>> GetSegmentsForMessageAsync(int messageId)
        {
            return await _context.Segments.Where(s => s.MessageId == messageId)
                                    .ToListAsync();
        }

        public async Task<Segment?> GetSegmentForMessageAsync(int messageId, int segmentId)
        {
            return await _context.Segments.Where(s => s.MessageId == messageId && s.Id == segmentId)
                                    .FirstOrDefaultAsync();
        }

        public async Task AddSegmentForMessageAsync(int messageId, Segment segment)
        {
            var message = await GetMessageAsync(messageId, false, false);
            if (message != null)
            {
                message.Segments.Add(segment);
            }
        }
        #endregion

        #region Files Repository Methods
        public async Task<IEnumerable<Entities.File>> GetFilesForMessageAsync(int messageId)
        {
            return await _context.Files.Where(f => f.MessageId == messageId)
                                    .ToListAsync();
        }
        public async Task<Entities.File?> GetFileForMessageAsync(int messageId, int fileId)
        {
            return await _context.Files.Where(f => f.MessageId == messageId && f.Id == fileId)
                                    .FirstOrDefaultAsync();
        }
        public async Task AddFileForMessageAsync(int messageId, Entities.File file)
        {
            var message = await GetMessageAsync(messageId, false, false);
            if (message != null)
            {
                message.Files.Add(file);
            }
        }
        #endregion

        #region Users Repository Methods

        public async Task<User?> GetUserAsync(string userName)
        {
            return await _context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<(User?, IEnumerable<Application>)> GetUserInfoForUsernameAsync(string userName)
        {
            var user = _context.Users.Where(u => u.UserName == userName);
            var applications = user.SelectMany(u => u.UserApplicationMaps.Select(uam => uam.Application)).ToList();
            
            return (await user.FirstOrDefaultAsync(), applications);
        }

        public async Task<User?> ValidateUserAsync(string userName, int applicationId)
        {
            var user = _context.Users.Where(u => u.UserName == userName)
                                        .Where(u => u.UserApplicationMaps
                                                    .Any(uam => uam.ApplicationId == applicationId 
                                                            && uam.Active == true));

            return await user.FirstOrDefaultAsync();
        }

        public async Task<UserApplicationMap?> GetUserApplicationMapAsync(int userId, int applicationId)
        {
            return await _context.UserApplicationMaps.Where(u => u.UserId == userId 
                                                                && u.ApplicationId == applicationId)
                                                     .FirstOrDefaultAsync(); 
        }
        #endregion

        #region Generic Repository Methods

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        #endregion
    }
}
