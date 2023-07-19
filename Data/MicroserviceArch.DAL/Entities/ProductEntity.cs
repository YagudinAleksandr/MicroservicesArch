using MicroserviceArch.Interfaces.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroserviceArch.DAL.Entities
{
    public class ProductEntity : IProductEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [ForeignKey("ClientEntity")]
        public string ClientId { get; set; }
        public ClientEntity Client { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Amount {  get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
