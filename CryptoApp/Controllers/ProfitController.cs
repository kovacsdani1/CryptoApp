using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProfitController : ControllerBase
{
    private readonly IProfitService _profitService;

    public ProfitController(IProfitService profitService)
    {
        _profitService = profitService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetProfit(int userId)
    {
        try
        {
            var profit = await _profitService.CalculateProfitAsync(userId);
            return Ok(profit);
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

    [HttpGet("details/{userId}")]
    public async Task<IActionResult> GetDetailedProfit(int userId)
    {
        try
        {
            var detailedProfit = await _profitService.CalculateDetailedProfitAsync(userId);
            return Ok(detailedProfit);
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