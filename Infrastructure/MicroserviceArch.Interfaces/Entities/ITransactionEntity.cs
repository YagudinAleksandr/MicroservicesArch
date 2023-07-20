using MicroserviceArch.Interfaces.Entities.Base;

namespace MicroserviceArch.Interfaces.Entities
{
    /// <summary>
    /// Интерфейс сущности транзакции
    /// </summary>
    public interface ITransactionEntity : IBaseEntity
    {
        /// <summary>
        /// Сумма транзакции
        /// </summary>
        double Sum { get; set; }
        /// <summary>
        /// Счет
        /// </summary>
        int CountId { get; set; }
        /// <summary>
        /// Счет получатель
        /// </summary>
        int CountReciverId { get; set; }
        /// <summary>
        /// Описание тразакции
        /// </summary>
        string Description { get; set; }
    }
}
