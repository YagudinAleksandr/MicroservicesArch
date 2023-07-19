using MicroserviceArch.DAL.Entities;
using MicroserviceArch.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceArch.DAL.Repositories
{
    public class CountRepository<T> : ICountRepository<T> where T : CountEntity
    {
        public Task<T> Add(T entity, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> Delete(int id, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> Get(int id, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAll(CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllByUser(int userId, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> Update(T entity, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }
    }
}
