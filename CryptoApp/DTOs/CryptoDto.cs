using CryptoApp.Entities;

namespace CryptoApp.DTOs
{
    public class CryptoDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
    }

    public class PostCryptoDto
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal Supply { get; set; }
    }

    public class UpdatePriceCryptoDto
    {
        public int cryptoId { get; set; }
        public decimal NewPrice { get; set; }
    }
}
