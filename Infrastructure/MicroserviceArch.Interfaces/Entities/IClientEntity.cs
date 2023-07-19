using MicroserviceArch.Interfaces.Entities.Base;

namespace MicroserviceArch.Interfaces.Entities
{
    /// <summary>
    /// Интерфейс сущности клиента
    /// </summary>
    public interface IClientEntity:IBaseEntity
    {
        /// <summary>
        /// E-mail пользователя
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// Полное имя
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Роль
        /// </summary>
        int RoleId { get; set; }
    }
}
