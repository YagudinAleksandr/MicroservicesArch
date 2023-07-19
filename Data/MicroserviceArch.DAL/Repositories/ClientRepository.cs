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
    public class ClientRepository<T> : IClientRepository<T> where T : ClientEntity, new()
    {
        #region Поля и Свойства
        private readonly DataDB db;
        protected DbSet<T> Set { get; }
        protected virtual IQueryable<T> Items => Set;
        #endregion

        public ClientRepository(DataDB db)
        {
            this.db = db;
            this.Set = this.db.Set<T>();
        }

        public async Task<T> Add(T entity, CancellationToken cancel = default)
        {
            if (entity == null || entity.RoleId == 0) throw new ArgumentNullException();

            var role = db.Roles.Where(i => i.Id == entity.RoleId).FirstOrDefault();

            if (role == null) throw new ArgumentNullException();

            entity.Role = role;

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
            await Items.FirstOrDefaultAsync(item => item.Id == id, cancel).ConfigureAwait(false);

        public async Task<List<T>> GetAll(CancellationToken cancel = default) =>
            await Items.ToListAsync(cancel).ConfigureAwait(false);

        public async Task<T> Update(T entity, CancellationToken cancel = default)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            entity.Role = null;

            db.Entry(entity).State = EntityState.Modified;

            await db.SaveChangesAsync(cancel).ConfigureAwait(false);

            return entity;
        }
    }
}
