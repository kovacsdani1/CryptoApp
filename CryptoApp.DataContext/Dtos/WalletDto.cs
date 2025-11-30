using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.DataContext.Dtos
{
    public class WalletDto
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public List<CryptoHoldingDto> Holdings { get; set; }
    }

    public class CryptoHoldingDto
    {
        public int CryptocurrencyId { get; set; }
        public string CryptoName { get; set; }
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePurchasePrice { get; set; }
    }
}
