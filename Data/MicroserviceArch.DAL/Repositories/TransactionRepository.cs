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
    public class TransactionRepository<T> : ITransactionRepository<T> where T : TransactionEntity, new()
    {
        #region Поля и Свойства
        private readonly DataDB db;
        protected DbSet<T> Set { get; }
        protected virtual IQueryable<T> Items => Set;
        #endregion
        public TransactionRepository(DataDB db)
        {
            this.db = db;
            this.Set = this.db.Set<T>();
        }

        public async Task<T> Add(T entity, CancellationToken cancel = default)
        {
            if (entity == null || entity.CountId == 0 || entity.CountReciverId == 0) throw new ArgumentNullException();

            var countSender = db.Counts.Where(i => i.Id == entity.CountId).FirstOrDefault();

            if (countSender == null) throw new ArgumentNullException();

            entity.Count = countSender;

            var countReciver = db.Counts.Where(i => i.Id == entity.CountReciverId).FirstOrDefault();

            if (countReciver == null) throw new ArgumentNullException();

            entity.CountReciver = countReciver;

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
            await Items.Include(c => c.Count).FirstOrDefaultAsync(item => item.Id == id, cancel).ConfigureAwait(false);

        public async Task<List<T>> GetAllByCount(int countId, CancellationToken cancel = default) =>
            await Items.Where(x => x.CountId == countId).Include(c => c.Count).ToListAsync(cancel).ConfigureAwait(false);
           
        public async Task<T> Update(T entity, CancellationToken cancel = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            entity.Count = null;
            entity.CountReciver = null;

            db.Entry(entity).State = EntityState.Modified;

            await db.SaveChangesAsync(cancel).ConfigureAwait(false);

            return entity;
        }

        
    }
}
