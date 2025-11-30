using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.DataContext.Dtos
{
    public class ProfitSummaryDto
    {
        public decimal TotalInvestment { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal ProfitPercentage { get; set; }
    }

    public class DetailedProfitDto
    {
        public int CryptocurrencyId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        public decimal AveragePurchasePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal Profit { get; set; }
        public decimal ProfitPercentage { get; set; }
    }
}
