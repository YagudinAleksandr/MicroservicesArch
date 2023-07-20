namespace MicroserviceArch.DAL.Repositories
{
    /// <summary>
    /// Модель DTO для валюты
    /// </summary>
    public class CurrencyRateDTO
    {
        public bool Success { get; set; }
        public string Query { get; set; }
        public string Info { get; set; }
        public string Historical { get; set; }
        public string Date { get; set; }
        public double Result { get; set; }
    }
}
