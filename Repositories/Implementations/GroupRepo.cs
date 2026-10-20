using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Repositories.Interfaces;

namespace Repositories.Implementations
{
    public class GroupRepo : IGroupInterface
    {
        private readonly IConnection _connection;

        public GroupRepo(IConnection connection)
        {
            _connection = connection;
        }


        // public async Task JoinGroup(string groupName, string user)
        // {
        //     using var channel = await _connection.CreateChannelAsync();

        //     string exchangeName = $"group_exchange_{groupName}";
        //     string queueName = $"group_{groupName}_{user}";

        //     await channel.ExchangeDeclareAsync(
        //         exchange: exchangeName,
        //         type: ExchangeType.Fanout
        //     );

        //     await channel.QueueDeclareAsync(
        //         queue: queueName,
        //         durable: false,
        //         exclusive: false,
        //         autoDelete: false
        //     );

        //     await channel.QueueBindAsync(
        //         queue: queueName,
        //         exchange: exchangeName,
        //         routingKey: ""
        //     );
        // }

        public async Task SendGroupMessage(string groupName, string sender, string message)
        {
            using var channel = await _connection.CreateChannelAsync();

            string exchangeName = $"group_exchange_{groupName}";

            await channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Fanout
            );

            string fullMessage = $"{sender}: {message}";
            var body = Encoding.UTF8.GetBytes(fullMessage);

            await channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: "",
                body: body
            );
        }

        public async Task<string> ReceiveGroupMessage(string groupName, string user)
        {
            using var channel = await _connection.CreateChannelAsync();

            string exchangeName = $"group_exchange_{groupName}";
            string queueName = $"group_{groupName}_{user}";

            await channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Fanout
            );

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false
            );

            await channel.QueueBindAsync(
                queue: queueName,
                exchange: exchangeName,
                routingKey: ""
            );

            var result = await channel.BasicGetAsync(queueName, true);

            if (result == null)
                return null;

            return Encoding.UTF8.GetString(result.Body.ToArray());
        }
    }
}