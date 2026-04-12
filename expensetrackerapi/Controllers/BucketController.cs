using expensetrackerapi.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace expensetrackerapi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BucketsController : ControllerBase
{
    private readonly ILogger<BucketsController> _logger;
    private readonly IBucketService _bucketService;

    public BucketsController(ILogger<BucketsController> logger, IBucketService bucketService)
    {
        // constructor DI 
        _logger = logger;
        _bucketService = bucketService;
    }

    [Authorize]
    [HttpGet("summary")]
    public async Task<ActionResult> Get([FromQuery] int month, [FromQuery] int year)
    {
        var transactions = await _bucketService.GetSummary(month, year);
        if (transactions.IsSuccess)
        {
            
            return Ok(transactions);
        }

        return BadRequest("No transactions found.");
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetBuckets()
    {
        var buckets = await _bucketService.GetBuckets();
        if (buckets.IsSuccess)
        {
            return Ok(buckets);
        }
        return BadRequest("No buckets found.");
    }
}
