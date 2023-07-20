using System;
using System.ComponentModel.DataAnnotations;

namespace MicroserviceArch.DTOEntity
{
    public class CountEntityDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Сумма не может быть пустым значением")]
        [Range(0.0, 500000, ErrorMessage ="Сумма может быть в пределах от 0 до 500000")]
        public double Count { get; set; }
        [Required(ErrorMessage = "Счет не может быть без клиента")]
        public int ClientId { get; set; }
        public string Client { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Notification { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
