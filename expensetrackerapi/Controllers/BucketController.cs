using System.Globalization;
using expensetrackerapi.Models;
using expensetrackerapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace expensetrackerapi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BucketsController : ControllerBase
{
    private readonly ILogger<BucketsController> _logger;
    private readonly IBucketService _service;

    public BucketsController(ILogger<BucketsController> logger, IBucketService service)
    {
        // constructor DI 
        _logger = logger;
        _service = service;
    }


    [HttpGet("summary")]
    public async Task<ActionResult> Get([FromQuery] int month, [FromQuery] int year)
    {
        var transactions = await _service.GetSummary(month, year);
        if (transactions.Buckets.Count > 0)
        {
            return Ok(transactions);
        }

        return BadRequest("No transactions found.");
    }
    
    [HttpGet]
    public async Task<ActionResult> GetBuckets()
    {
        var buckets = await _service.GetBuckets();
        if (buckets.Count > 0)
        {
            return Ok(buckets);
        }
        return BadRequest("No buckets found.");
    }
}
