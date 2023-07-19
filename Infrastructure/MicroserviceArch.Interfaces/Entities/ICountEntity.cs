using MicroserviceArch.Interfaces.Entities.Base;

namespace MicroserviceArch.Interfaces.Entities
{
    /// <summary>
    /// Интерфейс сущности счета
    /// </summary>
    public interface ICountEntity : IBaseEntity
    {
        /// <summary>
        /// Средства
        /// </summary>
        double Count { get; set; }
        /// <summary>
        /// Клиент
        /// </summary>
        int ClientId { get; set; }
    }
}
