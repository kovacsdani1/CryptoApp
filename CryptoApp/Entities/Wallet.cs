using System.ComponentModel.DataAnnotations;

namespace CryptoApp.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        [Required]
        public decimal Balance { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public List<Portfolio> Portfolios { get; set; }
    }
}
