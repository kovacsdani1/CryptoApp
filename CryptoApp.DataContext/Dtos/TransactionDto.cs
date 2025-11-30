using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.DataContext.Dtos
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string CryptoName { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
