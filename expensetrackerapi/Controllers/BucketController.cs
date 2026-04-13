using expensetrackerapi.Contracts;
using expensetrackerapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace expensetrackerapi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class BucketsController : ControllerBase
{
    private readonly ILogger<BucketsController> _logger;
    private readonly IBucketService _bucketService;
    private readonly UserManager<ApplicationUser> _manager;

    public BucketsController(ILogger<BucketsController> logger, IBucketService bucketService, UserManager<ApplicationUser> userManager)
    {
        // constructor DI 
        _logger = logger;
        _bucketService = bucketService;
        _manager = userManager;
    }

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
    [HttpGet("user")]
    public async Task<ActionResult> GetBucketsByUserId()
    {
        var id = _manager.GetUserId(User);
        var buckets = await _bucketService.GetBucketsByUserId(id);
        if (buckets.IsSuccess)
        {
            return Ok(buckets);
        }
        return BadRequest("No buckets found.");
    }
}
