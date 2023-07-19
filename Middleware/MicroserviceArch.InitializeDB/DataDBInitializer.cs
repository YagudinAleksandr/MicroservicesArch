using MicroserviceArch.DAL.Context;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using MicroserviceArch.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceArch.InitializeDB
{
    /// <summary>
    /// Класс инициализации первичных данных
    /// </summary>
    public class DataDBInitializer
    {
        private readonly DataDB db;
        public DataDBInitializer(DataDB db) => this.db = db;

        public void Initialize()
        {
            db.Database.Migrate();

            HasAnyRoles();
            HasAnyUsers();
        }

        /// <summary>
        /// Проверяет имеются ли роли в БД
        /// </summary>
        private void HasAnyRoles()
        {
            if (!db.Roles.Any())
            {
                List<RoleEntity> roles = new List<RoleEntity>
                {
                    new RoleEntity() 
                    { 
                        Name = "Сотрудник банка",
                        Description = "Сотрудник банка",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow 
                    },
                    new RoleEntity() 
                    { 
                        Name = "Клиент",
                        Description = "Клиент банка",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                db.AddRange(roles);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Проверяет имеются ли пользователи в БД
        /// </summary>
        private void HasAnyUsers()
        {
            RoleEntity role = db.Roles.Where(x => x.Id == 1).Where(n => n.Name == "Сотрудник банка").FirstOrDefault();

            if (!db.Clients.Any())
            {
                List<ClientEntity> users = new List<ClientEntity>
                {
                    new ClientEntity() 
                    {
                        Id = 1,
                        Email = "bank@bank.ru",
                        Password = "c9vjCdm7",
                        Name = "Петров Пётр Петрович",
                        RoleId = role.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow 
                    }
                };

                db.AddRange(users);
                db.SaveChanges();
            }
        }
    }
}
