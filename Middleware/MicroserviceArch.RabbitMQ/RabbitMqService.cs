using Newtonsoft.Json;
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
        public void SendMessage(MessageDTO messageDTO)
        {
            var factory = new ConnectionFactory() { HostName = $"{messageDTO.Host}" };

            string message = JsonConvert.SerializeObject(messageDTO);

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: $"{messageDTO.Type}{messageDTO.ClientID}",
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                               routingKey: $"{messageDTO.Type}{messageDTO.ClientID}",
                               basicProperties: null,
                               body: body);
            }
        }
        public async Task SendMessageAsync(MessageDTO messageDTO)
        {
            await Task.Run(() => SendMessage(messageDTO));
        }
    }
}
