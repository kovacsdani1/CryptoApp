using AutoMapper;
using CryptoApp.DTOs;
using CryptoApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public WalletController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves the wallet information associated with the specified user identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose wallet information is to be retrieved.</param>
        /// <returns>A walletDto containing the wallet details for the specified user if found;
        /// otherwise, a 404 Not Found response if the user does not exist.</returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<WalletDto>> GetWalletByUserId(int userId)
        {
            if (!await _unitOfWork.UserRepository.UserExistsAsync(userId))
                return NotFound("User not found.");

            var wallet = await _unitOfWork.WalletRepository.GetWalletAsync(userId);
            return _mapper.Map<WalletDto>(wallet);
        }

        /// <summary>
        /// Updates the balance of the specified user's wallet by the given amount.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose wallet balance will be updated.</param>
        /// <param name="amount">The amount of the new balance.</param>
        /// <returns>A status code</returns>
        [HttpPut("{userId},{amount}")]
        public async Task<ActionResult> UpdateBalance(int userId, decimal amount)
        {
            if (!await _unitOfWork.UserRepository.UserExistsAsync(userId))
                return NotFound("User not found.");

            await _unitOfWork.WalletRepository.UpdateBalanceAsync(userId, amount);
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        /// <summary>
        /// Deletes the wallet associated with the specified user identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose wallet is to be deleted.</param>
        /// <returns>A status code</returns>
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteWallet(int userId)
        {
            if (!await _unitOfWork.UserRepository.UserExistsAsync(userId))
                return NotFound("User not found.");

            await _unitOfWork.WalletRepository.DeleteWalletAsync(userId);
            return NoContent();

            //todo: delete portfolios related to wallet
        }
    }
}
