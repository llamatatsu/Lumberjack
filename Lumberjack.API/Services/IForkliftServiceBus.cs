using Lumberjack.API.Entities;

namespace Lumberjack.API.Services
{
    public interface IForkliftServiceBus
    {
        Task<bool> WriteMessageToServiceBus(Message message);

        //Task ReadMessagesFromServiceBusAsync();
    }
}