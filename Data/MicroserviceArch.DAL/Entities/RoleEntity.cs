using MicroserviceArch.Interfaces.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace MicroserviceArch.DAL.Entities
{
    public class RoleEntity : IRoleEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
