using MicroserviceArch.Interfaces.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroserviceArch.DAL.Entities
{
    public class CountEntity : ICountEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double Count { get; set; }
        [ForeignKey("ClientEntity")]
        public int ClientId { get; set; }
        public ClientEntity Client { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
