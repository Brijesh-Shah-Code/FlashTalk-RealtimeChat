using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Repositories.Interfaces;

namespace Repositories.Implementations
{
    public class MessageRepo : IMessageInterface
    {
        private readonly ConnectionFactory _factory;
        private readonly ILogger<MessageRepo> _logger;

        public MessageRepo(IConfiguration config, ILogger<MessageRepo> logger)
        {
            _logger = logger;
            _factory = new ConnectionFactory()
            {
                HostName = config["RabbitMQ:Host"],
                Port = int.Parse(config["RabbitMQ:Port"]),
                UserName = config["RabbitMQ:Username"],
                Password = config["RabbitMQ:Password"],
                VirtualHost = config["RabbitMQ:VirtsualHost"]
            };
        }

        public async Task SendMessage(string toEmail, string message)
        {
            try
            {
                using var connection = await _factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                // Explicitly declaring a durable queue
                await channel.QueueDeclareAsync(queue: toEmail, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);
                await channel.BasicPublishAsync(exchange: "", routingKey: toEmail, body: body);

                _logger.LogInformation($"Message sent to queue: {toEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SendMessage: {ex.Message}");
                throw;
            }
        }

        public async Task<string> ReceiveMessage(string userEmail)
        {
            try
            {
                using var connection = await _factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: userEmail, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var result = await channel.BasicGetAsync(queue: userEmail, autoAck: true);
                return result != null ? Encoding.UTF8.GetString(result.Body.ToArray()) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ReceiveMessage: {ex.Message}");
                return null;
            }
        }
    }
}