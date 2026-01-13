using AutoMapper;
using CryptoApp.DTOs;
using CryptoApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptosController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public CryptosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of all available cryptocurrencies.
        /// </summary>
        /// <returns>An a list of all cryptocurrencies. Returns an empty list if no cryptocurrencies are found.</returns>
        [HttpGet]
        public async Task<ActionResult<List<CryptoDto>>> GetAllCryptosAsync()
        {
            var cryptos = await _unitOfWork.CryptoRepository.GetAllCryptosAsync();
            return Ok(_mapper.Map<List<CryptoDto>>(cryptos));
        }

        /// <summary>
        /// Retrieves the details of a cryptocurrency with the specified identifier.
        /// </summary>
        /// <param name="cryptoId">The unique identifier of the cryptocurrency to retrieve.</param>
        /// <returns>A cryptocurrency details if found; otherwise, a 404 Not Found response.</returns>
        [HttpGet("{cryptoId}")]
        public async Task<ActionResult<CryptoDto>> GetCryptoByIdAsync(int cryptoId)
        {
            var crypto = await _unitOfWork.CryptoRepository.GetCryptoByIdAsync(cryptoId);
            if (crypto == null)
                return NotFound("Crypto not found.");
            return Ok(_mapper.Map<CryptoDto>(crypto));
        }

        /// <summary>
        /// Adds a new cryptocurrency to the system.
        /// </summary>
        /// <param name="cryptoDto">The data transfer object containing the details of the cryptocurrency to add.</param>
        /// <returns>A status code</returns>
        [HttpPost]
        public async Task<ActionResult> AddCryptoAsync(PostCryptoDto cryptoDto)
        {
            if(await _unitOfWork.CryptoRepository.IsCryptoSymbolExistsAsync(cryptoDto.Symbol))
                return Conflict("Crypto with the same symbol already exists."); 

            var crypto = _mapper.Map<Crypto>(cryptoDto);
            await _unitOfWork.CryptoRepository.AddCryptoAsync(crypto);
            await _unitOfWork.SaveAsync();
            return Created();
        }

        /// <summary>
        /// Deletes the cryptocurrency with the specified identifier.
        /// </summary>
        /// <param name="cryptoId">The unique identifier of the cryptocurrency to delete.</param>
        /// <returns>A status code</returns>
        [HttpDelete("{cryptoId}")]
        public async Task<ActionResult> DeleteCryptoAsync(int cryptoId)
        {
            var crypto = await _unitOfWork.CryptoRepository.GetCryptoByIdAsync(cryptoId);
            if (crypto == null)
                return NotFound("Crypto not found.");

            await _unitOfWork.CryptoRepository.DeleteCryptoAsync(cryptoId);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Updates the price of a cryptocurrency asset with the specified values.
        /// </summary>
        /// <param name="cryptodto">An object containing the identifier of the cryptocurrency to update and the new price to set. The new price
        /// must be greater than zero.</param>
        /// <returns>An <see cref="ActionResult"/> indicating the result of the operation. Returns <see cref="OkObjectResult"/>
        /// if the price is updated successfully, <see cref="NotFoundObjectResult"/> if the cryptocurrency is not found,
        /// or <see cref="BadRequestObjectResult"/> if the new price is not valid.</returns>
        [HttpPut("price")]
        public async Task<ActionResult> UpdatePriceAsync(UpdatePriceCryptoDto cryptodto)
        {
            var crypto = await _unitOfWork.CryptoRepository.GetCryptoByIdAsync(cryptodto.cryptoId);
            if(crypto == null)
                return NotFound("Crypto not found.");
            if (cryptodto.NewPrice <= 0)
                return BadRequest("Price must be greater than zero.");
            await _unitOfWork.CryptoRepository.UpdatePriceAsync(crypto, cryptodto.NewPrice);
            await _unitOfWork.SaveAsync();
            return Ok("Price updated successfully");
        }

        /// <summary>
        /// Retrieves the price history for a specific cryptocurrency by its unique identifier.
        /// </summary>
        /// <param name="cryptoId">The unique identifier of the cryptocurrency for which to retrieve price history.</param>
        /// <returns>A status code</returns>
        [HttpGet("price/history/{cryptoId}")]
        public async Task<ActionResult> GetPriceHistoryByCryptoIdAsnyc(int cryptoId)
        {
            if (await _unitOfWork.CryptoRepository.GetCryptoByIdAsync(cryptoId) == null)
                return NotFound("Crypto not found");

            var result = _mapper.Map<List<PriceHistoryDto>>(await _unitOfWork.CryptoRepository.GetPriceHistoryByIdAsync(cryptoId));

            return Ok(result);
        }
    }
}
