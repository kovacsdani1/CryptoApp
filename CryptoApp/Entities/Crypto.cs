using System.ComponentModel.DataAnnotations;

namespace CryptoApp.Entities
{
    public class Crypto
    {
        public int Id { get; set; }
        [Required]
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal Supply { get; set; }
        public List<Portfolio> Portfolios { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<PriceHistory> PriceHistory { get; set; }
    }
}
