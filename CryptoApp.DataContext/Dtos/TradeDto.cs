using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.DataContext.Dtos
{
    public class TradeRequestDto
    {
        public int UserId { get; set; }
        public int CryptocurrencyId { get; set; }
        public decimal Amount { get; set; }
    }
}
