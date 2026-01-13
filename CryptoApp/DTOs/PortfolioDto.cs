using CryptoApp.Entities;

namespace CryptoApp.DTOs
{
    // DTO representing a user's portfolio holding a specific cryptocurrency
    public class PortfolioDto
    {
        public decimal Amount { get; set; }
        public int WalletId { get; set; }
        public int CryptoId { get; set; }
    }
}
