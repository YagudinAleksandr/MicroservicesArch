using System.Collections.Generic;

namespace MicroserviceArch.Pagination.Entities
{
    /// <summary>
    /// Ответ страницы с постраничным отображением
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagingResponse<T>
    {
        /// <summary>
        /// Элементы страницы
        /// </summary>
        public List<T> Items { get; set; }
        /// <summary>
        /// Метаданные страницы
        /// </summary>
        public MetaData MetaData { get; set; }
    }
}
