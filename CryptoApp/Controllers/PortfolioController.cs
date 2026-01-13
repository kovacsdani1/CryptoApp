using AutoMapper;
using CryptoApp.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public PortfolioController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves the list of portfolio items associated with the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose portfolio is to be retrieved.</param>
        /// <returns>A status code</returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<List<PortfolioDto>>> GetPortfolioByUserIdAsync(int userId)
        {
            var wallet = await _unitOfWork.WalletRepository.GetWalletAsync(userId);
            if(wallet == null)
            {
                return NotFound("Wallet not found");
            }

            return Ok(_mapper.Map<List<PortfolioDto>>(wallet.Portfolios));
        }
    }
}
