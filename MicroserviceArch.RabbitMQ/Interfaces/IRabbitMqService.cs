namespace MicroserviceArch.RabbitMQ.Interfaces
{
    public interface IRabbitMqService
    {
        void SendMessage(object obj);
        void SendMessage(string message);
    }
}
