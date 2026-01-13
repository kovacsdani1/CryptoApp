using CryptoApp.Entities;

namespace CryptoApp.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string CryptoSymbol { get; set; }
    }
    public class TransactionDetailsDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public string Username { get; set; }
        public string CryptoSymbol { get; set; }
    }
}
