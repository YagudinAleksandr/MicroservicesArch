using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceArch.RabbitMQ
{
    /// <summary>
    /// Ревлизация
    /// </summary>
    public class RabbitMqService : IRabbitMqService
    {
        public void SendMessage(string message, string reciver, string address)
        {
            var factory = new ConnectionFactory() { HostName = $"{address}" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: $"{reciver}",
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                               routingKey: $"{reciver}",
                               basicProperties: null,
                               body: body);
            }
        }
        public async Task SendMessageAsync(string message, string reciver, string address)
        {
            await Task.Run(() => SendMessage(message, reciver, address));
        }
    }
}
