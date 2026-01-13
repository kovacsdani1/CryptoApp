namespace CryptoApp.DTOs
{
    // DTO for trade operations (buy/sell)
    public class BuyDto
    {
        public int UserId { get; set; }
        public string Symbol { get; set; }
        public decimal Budget { get; set; } // Amount user wants to spend (in money)
    }
    public class SellDto
    {
        public int UserId { get; set; }
        public string Symbol { get; set; }
        public decimal Amount { get; set; } // Amount user wants to sell (in crypto)
    }
}
