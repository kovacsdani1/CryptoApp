namespace CryptoApp.Entities
{
    public class Portfolio
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int WalletId { get; set; }
        public Wallet? Wallet { get; set; }
        public int CryptoId { get; set; }
        public Crypto? Crypto { get; set; }
    }
}
