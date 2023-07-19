using System;

namespace MicroserviceArch.Interfaces.Entities.Base
{
    /// <summary>
    /// Интерфейс базовой сущности
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// ID сущности
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// Дата создания сущности
        /// </summary>
        DateTimeOffset CreatedAt { get; set; }
        /// <summary>
        /// Дата изменения сущности
        /// </summary>
        DateTimeOffset UpdatedAt { get; set; }
    }
}
