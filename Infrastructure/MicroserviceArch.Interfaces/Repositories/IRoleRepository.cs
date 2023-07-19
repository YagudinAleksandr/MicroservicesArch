using MicroserviceArch.Interfaces.Entities;
using MicroserviceArch.Interfaces.Repositories.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceArch.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория ролей
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRoleRepository<T> : IBaseRepository<T> where T : IRoleEntity
    {
        /// <summary>
        /// Получение всех сущностей
        /// </summary>
        /// <param name="cancel">Токен отмены</param>
        /// <returns>Список сущностей</returns>
        Task<List<T>> GetAll(CancellationToken cancel = default);
    }
}
