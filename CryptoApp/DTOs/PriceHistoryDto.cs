using CryptoApp.Entities;

namespace CryptoApp.DTOs
{
    public class PriceHistoryDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal NewPrice { get; set; }
        public string CryptoSymbol { get; set; }
    }
}
