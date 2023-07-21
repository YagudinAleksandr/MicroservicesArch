using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace MicroserviceArch.CheckRebbitMQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите хост:");
            string host = Console.ReadLine();

            Console.WriteLine("Введите тип сообщений (Transaction):");
            string messageType = Console.ReadLine();

            Console.WriteLine("Введите ID клиента:");
            string clientId = Console.ReadLine();

            var factory = new ConnectionFactory() { HostName = $"{host}" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: $"{messageType}{clientId}",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Получено уведомление: {0}", message);
                };
                channel.BasicConsume(queue: $"{messageType}{clientId}",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Нажмите [enter] для выхода");
                Console.ReadLine();
            }

        }
    }
}
