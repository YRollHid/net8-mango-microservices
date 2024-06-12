using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly IConfiguration _configuration;
        private string connectionString = "ServiceBusConnectionString";

        public string? ConnectionString { get; private set; }

        public MessageBus(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            ConnectionString = this._configuration["ServiceBusConnectionString"];
            await using var client = new ServiceBusClient(ConnectionString);

            ServiceBusSender sender = client.CreateSender(topic_queue_Name);

            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding
                .UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
