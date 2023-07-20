using MicroserviceArch.DAL.Context;
using MicroserviceArch.DAL.Entities;
using MicroserviceArch.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceArch.DAL.Repositories
{
    public class CountRepository<T> : ICountRepository<T> where T : CountEntity, new()
    {
        #region Поля и Свойства
        private readonly DataDB db;
        protected DbSet<T> Set { get; }
        protected virtual IQueryable<T> Items => Set;
        #endregion
        public CountRepository(DataDB db)
        {
            this.db = db;
            this.Set = this.db.Set<T>();
        }

        public async Task<T> Add(T entity, CancellationToken cancel = default)
        {
            if (entity == null || entity.ClientId == 0) throw new ArgumentNullException();

            var client = db.Clients.Where(i => i.Id == entity.ClientId).FirstOrDefault();

            if (client == null) throw new ArgumentNullException();

            entity.Client = client;

            await db.AddAsync(entity, cancel).ConfigureAwait(false);

            await db.SaveChangesAsync(cancel).ConfigureAwait(false);

            return entity;
        }

        public async Task<T> Delete(int id, CancellationToken cancel = default)
        {
            var item = Set.Local.FirstOrDefault(x => x.Id == id);

            if (item == null)
                item = await Set.Select(i => new T { Id = i.Id })
                    .FirstOrDefaultAsync(x => x.Id == id, cancel)
                    .ConfigureAwait(false);

            if (item is null) return null;

            db.Entry(item).State = EntityState.Deleted;

            await db.SaveChangesAsync(cancel).ConfigureAwait(false);

            return item;
        }

        public async Task<T> Get(int id, CancellationToken cancel = default) =>
            await Items.Include(c=>c.Client).FirstOrDefaultAsync(item => item.Id == id, cancel).ConfigureAwait(false);

        public async Task<List<T>> GetAll(CancellationToken cancel = default) =>
            await Items.Include(c => c.Client).ToListAsync(cancel).ConfigureAwait(false);

        public async Task<List<T>> GetAllByUser(int userId, CancellationToken cancel = default) =>
            await Items.Where(x => x.ClientId == userId).Include(c => c.Client).ToListAsync(cancel).ConfigureAwait(false);

        public async Task<T> Update(T entity, CancellationToken cancel = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            entity.Client = null;

            db.Entry(entity).State = EntityState.Modified;

            await db.SaveChangesAsync(cancel).ConfigureAwait(false);

            return entity;
        }

        public async Task<bool> ExsistCount(int countId, CancellationToken cancel = default) =>
            await Items.AnyAsync(item => item.Id == countId, cancel).ConfigureAwait(false);

        public async Task<bool> CheckBalanceForTransaction(int countId, double sum, CancellationToken cancel = default)
        {
            var count = await Items.FirstOrDefaultAsync(item => item.Id == countId, cancel).ConfigureAwait(false);

            return count.Count >= sum ? true : false;
        }

        public async Task<bool> ExistsCounts(int clientId, CancellationToken cancel = default)=>
            await Items.AnyAsync(item => item.ClientId == clientId, cancel).ConfigureAwait(false);
        
    }
}
