using MicroserviceArch.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceArch.DAL.Context
{
    public class DataDB : DbContext
    {
        public DataDB(DbContextOptions<DataDB> options) : base(options) { }

        /// <summary>
        /// Роли
        /// </summary>
        public DbSet<RoleEntity> Roles { get; set; }
        /// <summary>
        /// Клиенты
        /// </summary>
        public DbSet<ClientEntity> Clients { get; set; }
        /// <summary>
        /// Счета
        /// </summary>
        public DbSet<CountEntity> Counts { get; set; }
        /// <summary>
        /// Транзакции
        /// </summary>
        public DbSet<TransactionEntity> Transactions { get; set; }
        /// <summary>
        /// Услуги/товары
        /// </summary>
        public DbSet<ProductEntity> Products { get; set; }
    }
}
