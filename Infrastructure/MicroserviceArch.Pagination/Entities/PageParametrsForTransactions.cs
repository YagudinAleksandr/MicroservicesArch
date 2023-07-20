using System;

namespace MicroserviceArch.Pagination.Entities
{
    public class PageParametrsForTransactions : PageParametrs
    {
        /// <summary>
        /// Id счета
        /// </summary>
        public int CountId { get; set; } 
        /// <summary>
        /// Дата начала сортировки
        /// </summary>
        public DateTime startDate { get; set; } = default;
        /// <summary>
        /// Дата окончания сортировки
        /// </summary>
        public DateTime endDate { get; set; } = default;
        /// <summary>
        /// Тип сортировки (0 - по дате, 1 - по возрастанию, 2 - по убыванию)
        /// </summary>
        public int IsSortStandart { get; set; } = 0;
    }
}
