using CryptoApp.DataContext.Dtos;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TradeController : ControllerBase
{
    private readonly ITradeService _tradeService;

    public TradeController(ITradeService tradeService)
    {
        _tradeService = tradeService;
    }

    [HttpPost("buy")]
    public async Task<IActionResult> BuyCrypto([FromBody] TradeRequestDto request)
    {
        try
        {
            var transaction = await _tradeService.BuyCryptoAsync(request);
            return Ok(transaction);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("sell")]
    public async Task<IActionResult> SellCrypto([FromBody] TradeRequestDto request)
    {
        try
        {
            var transaction = await _tradeService.SellCryptoAsync(request);
            return Ok(transaction);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}