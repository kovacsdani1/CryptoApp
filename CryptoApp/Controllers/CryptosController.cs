using CryptoApp.DataContext.Dtos;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CryptosController : ControllerBase
{
    private readonly ICryptocurrencyService _cryptoService;

    public CryptosController(ICryptocurrencyService cryptoService)
    {
        _cryptoService = cryptoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCryptocurrencies()
    {
        try
        {
            var cryptos = await _cryptoService.GetAllCryptocurrenciesAsync();
            return Ok(cryptos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{cryptoId}")]
    public async Task<IActionResult> GetCryptocurrency(int cryptoId)
    {
        try
        {
            var crypto = await _cryptoService.GetCryptocurrencyByIdAsync(cryptoId);
            return Ok(crypto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCryptocurrency([FromBody] CryptocurrencyDto cryptoDto)
    {
        try
        {
            var crypto = await _cryptoService.CreateCryptocurrencyAsync(cryptoDto);
            return CreatedAtAction(nameof(GetCryptocurrency), new { cryptoId = crypto.Id }, crypto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{cryptoId}")]
    public async Task<IActionResult> DeleteCryptocurrency(int cryptoId)
    {
        try
        {
            await _cryptoService.DeleteCryptocurrencyAsync(cryptoId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("price")]
    public async Task<IActionResult> UpdatePrice([FromBody] dynamic request)
    {
        try
        {
            int cryptoId = request.cryptoId;
            decimal newPrice = request.newPrice;

            var crypto = await _cryptoService.UpdatePriceAsync(cryptoId, newPrice);
            return Ok(crypto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("price/history/{cryptoId}")]
    public async Task<IActionResult> GetPriceHistory(int cryptoId)
    {
        try
        {
            var history = await _cryptoService.GetPriceHistoryAsync(cryptoId);
            return Ok(history);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}