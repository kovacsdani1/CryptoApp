using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.DataContext.Entities
{
    public enum TransactionType
    {
        Buy,
        Sell
    }
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CryptocurrencyId { get; set; }
        public Cryptocurrency Cryptocurrency { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
