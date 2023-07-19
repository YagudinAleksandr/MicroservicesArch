using MicroserviceArch.Interfaces.Entities.Base;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceArch.Interfaces.Repositories.Base
{
    /// <summary>
    /// Базовый интерфейс репозитория
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository <T> where T : IBaseEntity
    {
        /// <summary>
        /// Добавление сущности
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Созданная сущность</returns>
        Task<T> Add(T entity, CancellationToken cancel = default);
        /// <summary>
        /// Обновление сущности
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Обновленная сущность</returns>
        Task<T> Update(T entity, CancellationToken cancel = default);
        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <param name="id">ID сущности</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Удаленная сущность</returns>
        Task<T> Delete(int id, CancellationToken cancel = default);
        /// <summary>
        /// Получение сущности
        /// </summary>
        /// <param name="id">ID сущности</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Сущность</returns>
        Task<T> Get(int id, CancellationToken cancel = default);
    }
}
