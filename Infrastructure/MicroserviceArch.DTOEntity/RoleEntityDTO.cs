using System;
using System.ComponentModel.DataAnnotations;

namespace MicroserviceArch.DTOEntity
{
    /// <summary>
    /// DTO модель роли
    /// </summary>
    public class RoleEntityDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле с названием должно быть заполнено")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле с названием должно быть заполнено")]
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Notification { get; set; }
        public bool IsSuccesful { get; set; }
    }
}
