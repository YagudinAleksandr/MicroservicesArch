using System.Threading.Tasks;

namespace MicroserviceArch.RabbitMQ
{
    /// <summary>
    /// Интерфейс реализации SendMessage
    /// </summary>
    public interface IRabbitMqService
    {
        /// <summary>
        /// Отправка сообщений асинхронно
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="reciver">Очередь для приема</param>
        /// <param name="address">Адрес сервера</param>
        Task SendMessageAsync(string message, string reciver, string address);
        /// <summary>
        /// Отправка сообщений
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="reciver">Очередь для приема</param>
        /// <param name="address">Адрес сервера</param>
        void SendMessage(string message, string reciver, string address);
    }
}
