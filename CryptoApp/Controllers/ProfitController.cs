using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfitController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProfitController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrieves the total profit associated with the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose profit is to be retrieved.</param>
        /// <returns>A status code</returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserProfit(int userId)
        {
            if(await _unitOfWork.UserRepository.GetUserAsync(userId) == null)
            {
                return NotFound("User not found");
            }

            return Ok(await _unitOfWork.ProfitRepository.CalulateProfitAsync(userId));
        }

        /// <summary>
        /// Retrieves detailed profit information for the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose profit details are to be retrieved. Must correspond to an existing
        /// user.</param>
        /// <returns>A status code</returns>
        [HttpGet("details/{userId}")]
        public async Task<ActionResult> GetUserProfitDetails(int userId)
        {
            if (await _unitOfWork.UserRepository.GetUserAsync(userId) == null)
            {
                return NotFound("User not found");
            }

            return Ok(await _unitOfWork.ProfitRepository.CalulcateProfitDetailsAsync(userId));
        }
    }
}
