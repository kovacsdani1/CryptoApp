using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.DataContext.Entities
{
    public class PriceHistory
    {
        public int Id { get; set; }
        public int CryptocurrencyId { get; set; }
        public Cryptocurrency Cryptocurrency { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
