using MicroserviceArch.Interfaces.Entities;
using MicroserviceArch.Interfaces.Repositories.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceArch.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория счетов
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICountRepository<T> : IBaseRepository<T> where T : ICountEntity
    {
        /// <summary>
        /// Получение всех счетов
        /// </summary>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Список сущностей</returns>
        Task<List<T>> GetAll(CancellationToken cancel = default);
        /// <summary>
        /// Получение всех сущностей по ID клиента
        /// </summary>
        /// <param name="userId">ID клиента</param>
        /// <param name="cancel">Отмена</param>
        /// <returns>Список сущностей</returns>
        Task<List<T>> GetAllByUser(int userId, CancellationToken cancel = default);
    }
}
