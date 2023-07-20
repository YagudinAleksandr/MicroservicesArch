using MicroserviceArch.Interfaces.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroserviceArch.DAL.Entities
{
    public class TransactionEntity : ITransactionEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double Sum { get; set; }
        [ForeignKey("CountEntity")]
        public int CountId { get; set; }
        public CountEntity Count { get; set; }
        [ForeignKey("CountEntity")]
        public int CountReciverId { get; set; }
        public CountEntity CountReciver { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        
    }
}
