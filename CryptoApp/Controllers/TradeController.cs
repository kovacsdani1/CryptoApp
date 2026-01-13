using CryptoApp.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public TradeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processes a request to purchase a specified amount of cryptocurrency for a user.
        /// </summary>
        /// <param name="buyDto">An object containing the details of the purchase, including the user ID, cryptocurrency symbol, and budget
        /// to spend. Must not be null.</param>
        /// <returns>A status code</returns>
        [HttpPost("buy")]
        public async Task<ActionResult> BuyCryptoAsync(BuyDto buyDto)
        {
            var crypto = await _unitOfWork.CryptoRepository.GetCryptoBySymbolAsync(buyDto.Symbol);
            if(crypto == null)
                return NotFound("Crypto not found");

            var user = await _unitOfWork.UserRepository.GetUserAsync(buyDto.UserId);
            if(user == null)
                return NotFound("User not found");

            var wallet = await _unitOfWork.WalletRepository.GetWalletAsync(buyDto.UserId);
            if (wallet == null)
                return NotFound("User has no wallet");

            if (wallet.Balance < buyDto.Budget)
                return BadRequest("Insufficient funds in wallet");

            decimal amountToBuy = buyDto.Budget / crypto.Price;

            if (crypto.Supply<amountToBuy)
                return BadRequest("Not enough supply of the cryptocurrency");

            await _unitOfWork.TradeRepository.BuyAsync(buyDto.UserId, crypto, wallet, amountToBuy, buyDto.Budget);
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        /// <summary>
        /// Processes a request to sell a specified amount of cryptocurrency for a user.
        /// </summary>
        /// <param name="sellDto">An object containing the details of the sell transaction, including the user ID, cryptocurrency symbol, and
        /// amount to sell. All fields must be valid and the user must have sufficient holdings.</param>
        /// <returns>A status code</returns>
        [HttpPost("sell")]
        public async Task<ActionResult> SellCryptoAsync(SellDto sellDto)
        {
            var crypto = await _unitOfWork.CryptoRepository.GetCryptoBySymbolAsync(sellDto.Symbol);
            if (crypto == null)
                return NotFound("Crypto not found");

            var user = await _unitOfWork.UserRepository.GetUserAsync(sellDto.UserId);
            if (user == null)
                return NotFound("User not found");

            var wallet = await _unitOfWork.WalletRepository.GetWalletAsync(sellDto.UserId);
            if (wallet == null)
                return NotFound("User has no wallet");

            var portfolio = wallet.Portfolios.FirstOrDefault(p => p.CryptoId == crypto.Id);
            if (portfolio == null || portfolio.Amount < sellDto.Amount)
            {
                return Conflict("User doesn't have enough crypto");
            }

            await _unitOfWork.TradeRepository.SellAsync(sellDto.UserId, crypto, wallet, sellDto.Amount, portfolio);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
