using System.Threading.Tasks;

namespace MicroserviceArch.RabbitMQ
{
    /// <summary>
    /// Интерфейс реализации SendMessage
    /// </summary>
    public interface IRabbitMqService
    {
        /// <summary>
        /// Асинхронная отправка сообщений в RabbitMQ
        /// </summary>
        /// <param name="messageDTO">Модель отправки</param>
        /// <returns></returns>
        Task SendMessageAsync(MessageDTO messageDTO);
        /// <summary>
        /// Отправка сообщений в RabbitMQ
        /// </summary>
        /// <param name="messageDTO">Модель отправки</param>
        void SendMessage(MessageDTO messageDTO);
    }
}
