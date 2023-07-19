using MicroserviceArch.Interfaces.Entities.Base;

namespace MicroserviceArch.Interfaces.Entities
{
    /// <summary>
    /// Интерфейс сущности роли
    /// </summary>
    public interface IRoleEntity : IBaseEntity
    {
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        string Description { get; set; }
    }
}
