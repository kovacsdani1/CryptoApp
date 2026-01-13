namespace CryptoApp.DTOs
{
    public class WalletDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public List<PortfolioDto> Portfolios { get; set; }
    }
}
