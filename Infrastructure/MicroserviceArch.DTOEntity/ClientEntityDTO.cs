using System;
using System.ComponentModel.DataAnnotations;

namespace MicroserviceArch.DTOEntity
{
    /// <summary>
    /// DTO модель клиента
    /// </summary>
    public class ClientEntityDTO
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "Поле Email должно быть заполнено")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Поле Пароль должно быть заполнено")]
        public string Password { get; set; }
        [Required (ErrorMessage = "Поле ФИО должно быть заполнено")]
        public string Name { get; set; }
        public int RoleId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Notification { get; set; }
        public bool IsSuccesfull { get; set; }
    }
}
