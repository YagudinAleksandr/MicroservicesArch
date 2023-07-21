using System;

namespace MicroserviceArch.RabbitMQ
{
    public class MessageDTO
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Тип сообщения
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Клиент
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// Хост RabbitMQ
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Дата сообщения
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }
    }
}
