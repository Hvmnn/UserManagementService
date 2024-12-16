using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UserManagementService.Src.Helpers;

namespace UserManagementService.Src.Services
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly RabbitMQOptions _options;

        public RabbitMQConsumer(IOptions<RabbitMQOptions> options)
        {
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.Host,
                Port = _options.Port,
                UserName = _options.Username,
                Password = _options.Password
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(_options.Exchange, ExchangeType.Direct, durable: true);
            channel.QueueDeclare(_options.QueueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;

                Console.WriteLine($"Message Received: {message} with Routing Key: {routingKey}");

                if (routingKey == _options.RoutingKeyUserCreated)
                {
                    Console.WriteLine("Processing user creation event...");
                }
                else if (routingKey == _options.RoutingKeyProgressUpdated)
                {
                    Console.WriteLine("Processing progress update event...");
                }
            };

            channel.BasicConsume(queue: _options.QueueName, autoAck: true, consumer: consumer);

            await Task.CompletedTask;
        }
    }
}