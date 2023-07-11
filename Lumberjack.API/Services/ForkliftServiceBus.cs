using Azure.Messaging.ServiceBus;
using Lumberjack.API.Entities;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Lumberjack.API.Services
{
    public class ForkliftServiceBus : IForkliftServiceBus
    {
        private readonly string _connectionString;
        private readonly string _queueName;

        public ForkliftServiceBus(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ServiceBusConnectionString");
            _queueName = configuration["ServiceBusSettings:QueueName"];
        }

        public async Task WriteMessageToServiceBus(Message message)
        {
            try
            {
                string jsonMessage = JsonConvert.SerializeObject(message, Formatting.Indented,
                                                                        new JsonSerializerSettings
                                                                        {
                                                                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                                        });

                await using (ServiceBusClient client = new ServiceBusClient(_connectionString))
                {
                    ServiceBusSender sender = client.CreateSender(_queueName);
                    ServiceBusMessage busMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage));
                    await sender.SendMessageAsync(busMessage);
                }
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException($"Exception while writing Message to Service bus", ex);
            }
        }
    }
}
