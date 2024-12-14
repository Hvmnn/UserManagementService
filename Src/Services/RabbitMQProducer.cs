using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using UserManagementService.Src.Helpers;

namespace UserManagementService.Src.Services
{
    public class RabbitMQProducer
    {
        private readonly RabbitMQOptions _options;

        public RabbitMQProducer(IOptions<RabbitMQOptions> options)
        {
            _options = options.Value;
        }

        public void Publish(string queueName, string message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.Host,
                Port = _options.Port,
                UserName = _options.Username,
                Password = _options.Password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            Console.WriteLine($"[x] Sent {message}");
        }
    }

}
