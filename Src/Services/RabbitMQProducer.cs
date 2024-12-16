using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using UserManagementService.Src.Helpers;

namespace UserManagementService.Src.Services
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly RabbitMQOptions _options;

        public RabbitMQProducer(IOptions<RabbitMQOptions> options)
        {
            _options = options.Value;
        }

        private void Publish(string routingKey, string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.Host,
                Port = _options.Port,
                UserName = _options.Username,
                Password = _options.Password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(_options.Exchange, ExchangeType.Direct, durable: true);
            channel.QueueDeclare(_options.QueueName, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(_options.QueueName, _options.Exchange, routingKey);

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: _options.Exchange,
                                routingKey: routingKey,
                                basicProperties: null,
                                body: body);
            Console.WriteLine($"Message Sent: {message}");
        }

        public void PublishUserCreated(string message)
        {
            Publish(_options.RoutingKeyUserCreated, message);
        }

        public void PublishProgressUpdated(string message)
        {
            Publish(_options.RoutingKeyProgressUpdated, message);
        }
    }

}
