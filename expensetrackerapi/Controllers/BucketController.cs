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


    [HttpGet("summary/{bucket?}")]
    public IActionResult Get(Buckets bucket, [FromQuery] int month, [FromQuery] int year)
    {
        var transactions = _service.GetSummary(bucket, month, year);
        if (transactions.Buckets.Count > 0)
        {
            return Ok(transactions);
        }

        return BadRequest("No transactions found.");
    }
    
    [HttpGet("{bucket?}")]
    public IActionResult GetBuckets(Buckets bucket)
    {
        var buckets = _service.GetBuckets(bucket);
        if (buckets.Count > 0)
        {
            return Ok(buckets);
        }

        return BadRequest("No buckets found.");
    }
}
