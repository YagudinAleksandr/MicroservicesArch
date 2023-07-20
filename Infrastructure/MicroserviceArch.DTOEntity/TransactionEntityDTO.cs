using System;
using System.ComponentModel.DataAnnotations;

namespace MicroserviceArch.DTOEntity
{
    public class TransactionEntityDTO
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "Сумма должна быть указана")]
        [Range(5,10000,ErrorMessage = "Сумма может быть в диапазоне от 5 до 10000")]
        public double Sum { get; set; }
        public int CountId { get; set; }
        [Required (ErrorMessage = "Укажите счет получателя")]
        public int CountReciverId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsSuccessful { get; set; }
        public string Notification { get; set; }
    }
}
