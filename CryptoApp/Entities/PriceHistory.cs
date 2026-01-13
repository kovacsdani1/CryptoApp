namespace CryptoApp.Entities
{
    public class PriceHistory
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal NewPrice { get; set; }
        public int CryptoId { get; set; }
        public Crypto? Crypto { get; set; }
    }
}
