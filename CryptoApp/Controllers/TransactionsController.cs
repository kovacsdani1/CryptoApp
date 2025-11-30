using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserTransactions(int userId)
    {
        try
        {
            var transactions = await _transactionService.GetUserTransactionsAsync(userId);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("details/{transactionId}")]
    public async Task<IActionResult> GetTransactionDetails(int transactionId)
    {
        try
        {
            var transaction = await _transactionService.GetTransactionDetailsAsync(transactionId);
            return Ok(transaction);
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