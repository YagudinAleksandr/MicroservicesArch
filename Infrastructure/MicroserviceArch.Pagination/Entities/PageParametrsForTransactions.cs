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
        /// Тип сортировки
        /// </summary>
        public bool IsSortStandart { get; set; } = default;
    }
}
