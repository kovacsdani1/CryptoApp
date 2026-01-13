using System.ComponentModel.DataAnnotations;

namespace CryptoApp.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public Wallet? Wallet { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
