using Lumberjack.API.Entities;

namespace Lumberjack.API.Services
{
    public interface IForkliftServiceBus
    {
        Task WriteMessageToServiceBus(Message message);
    }
}