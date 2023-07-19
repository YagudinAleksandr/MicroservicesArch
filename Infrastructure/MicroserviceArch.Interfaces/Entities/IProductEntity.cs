using MicroserviceArch.Interfaces.Entities.Base;

namespace MicroserviceArch.Interfaces.Entities
{
    /// <summary>
    /// Интерфейс сущности продукции
    /// </summary>
    internal interface IProductEntity : IBaseEntity
    {
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Клиент
        /// </summary>
        string ClientId { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        double Price { get; set; }
        /// <summary>
        /// Валюта
        /// </summary>
        string Amount { get; set; }
    }
}
