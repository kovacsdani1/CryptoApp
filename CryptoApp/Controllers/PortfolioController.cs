using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class PortfolioController : ControllerBase
{
    private readonly ITradeService _tradeService;

    public PortfolioController(ITradeService tradeService)
    {
        _tradeService = tradeService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetPortfolio(int userId)
    {
        try
        {
            var portfolio = await _tradeService.GetPortfolioAsync(userId);
            return Ok(portfolio);
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