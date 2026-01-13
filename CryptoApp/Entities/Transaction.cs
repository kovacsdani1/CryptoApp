namespace CryptoApp.Entities
{
    public enum TransactionType
    {
        Buy,
        Sell
    }
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int CryptoId { get; set; }
        public Crypto? Crypto { get; set; }
    }
}
