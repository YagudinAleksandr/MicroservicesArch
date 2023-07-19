using MicroserviceArch.Interfaces.Entities;
using MicroserviceArch.Interfaces.Repositories.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceArch.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория транзакций
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITransactionRepository<T> : IBaseRepository<T> where T : ITransactionEntity
    {
        /// <summary>
        /// Получение всех записей по счету
        /// </summary>
        /// <param name="countId">ID счета</param>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Список</returns>
        Task<List<T>> GetAllByCount(int countId, CancellationToken cancel = default);
        /// <summary>
        /// Получение всех транакций
        /// </summary>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Список</returns>
        Task<List<T>> GetAll(CancellationToken cancel = default);
    }
}
