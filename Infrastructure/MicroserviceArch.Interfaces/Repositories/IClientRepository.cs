using MicroserviceArch.Interfaces.Entities;
using MicroserviceArch.Interfaces.Repositories.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceArch.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория клиентов
    /// </summary>
    public interface IClientRepository<T> : IBaseRepository<T> where T : IClientEntity
    {
        /// <summary>
        /// Получение всех сущностей
        /// </summary>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Список сущностей</returns>
        Task<List<T>> GetAll(CancellationToken cancel = default);
        /// <summary>
        /// Метод входа в приложение
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="cancel">Отмена</param>
        /// <returns>Пользователь</returns>
        Task<T> GetUser(string login, string password, CancellationToken cancel = default);
    }
}
