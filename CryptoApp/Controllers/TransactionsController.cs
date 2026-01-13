using AutoMapper;
using CryptoApp.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public TransactionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all transactions associated with the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose transactions are to be retrieved. Must correspond to an existing
        /// user.</param>
        /// <returns>A status code</returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetTransactionsByUserIdAsync(int userId)
        {
            if(await _unitOfWork.UserRepository.UserExistsAsync(userId) == false)
            {
                return NotFound("User not found");
            }

            var transactions = await _unitOfWork.TransactionRepository.GetTransactionsByUserIdAsync(userId);
            var result = _mapper.Map<List<TransactionDto>>(transactions);
            return Ok(result);
        }

        [HttpGet("details/{transactionId}")]
        public async Task<ActionResult> GetTransactionByIdAsync(int transactionId)
        {
            var transaction = await _unitOfWork.TransactionRepository.GetTransactionByIdAsync(transactionId);
            if(transaction == null)
            {
                return NotFound("Transaction not found");
            }

            var result = _mapper.Map<TransactionDetailsDto>(transaction);
            return Ok(result);
        }
    }
}
