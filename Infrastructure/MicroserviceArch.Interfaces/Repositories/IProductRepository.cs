using MicroserviceArch.Interfaces.Entities;
using MicroserviceArch.Interfaces.Repositories.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceArch.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория продуктов/услуг
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProductRepository<T> : IBaseRepository<T> where T : IProductEntity
    {
        /// <summary>
        /// Получение всех услуг и продукции
        /// </summary>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Список продукции</returns>
        Task<List<T>> GetAll(CancellationToken cancel = default);
    }
}
