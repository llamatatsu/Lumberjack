using Azure.Messaging.ServiceBus;
using Lumberjack.API.Entities;
using System.Text;
using Newtonsoft.Json;

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

        public async Task<bool> WriteMessageToServiceBus(Message message)
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

                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        /* TODO: Figure out why the receiver is not getting latest Messages

        public async Task ReadMessagesFromServiceBusAsync()
        {
            await using (ServiceBusClient client = new ServiceBusClient(_connectionString))
            {
                ServiceBusProcessor processor = client.CreateProcessor(_queueName, new ServiceBusProcessorOptions());
                processor.ProcessMessageAsync += ProcessMessageAsync;
                processor.ProcessErrorAsync += ErrorHandler;
                await processor.StartProcessingAsync();

                Console.WriteLine("Receiving messages from Azure Service Bus");
                //Console.ReadKey();

                await processor.StopProcessingAsync();
                Console.WriteLine("Message receiving stopped.");
            }
        }

        private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            string message = Encoding.UTF8.GetString(args.Message.Body);
            Console.WriteLine($"Received message: {message}");

            // Do something with the message
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Error occurred: {args.Exception}");
            return Task.CompletedTask;
        }
        */
    }
}
