using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetWallet(int userId)
    {
        try
        {
            var wallet = await _walletService.GetWalletAsync(userId);
            return Ok(wallet);
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

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateBalance(int userId, [FromBody] decimal newBalance)
    {
        try
        {
            var wallet = await _walletService.UpdateBalanceAsync(userId, newBalance);
            return Ok(wallet);
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

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteWallet(int userId)
    {
        try
        {
            await _walletService.DeleteWalletAsync(userId);
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
}