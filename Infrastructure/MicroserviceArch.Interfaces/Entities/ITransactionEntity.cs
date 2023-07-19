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
        /// Является зачислением (True) или расходом (False)
        /// </summary>
        bool IsComing { get; set; }
        /// <summary>
        /// Счет
        /// </summary>
        int CountId { get; set; }
        /// <summary>
        /// Клиент отправитель
        /// </summary>
        int? ClientSenderId { get; set; }
        /// <summary>
        /// Клиент получатель
        /// </summary>
        int? ClientReciverId { get; set; }
        /// <summary>
        /// Описание тразакции
        /// </summary>
        string Description { get; set; }
    }
}
