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


    [HttpGet("{bucket?}")]
    public IActionResult Get(Buckets? bucket, [FromQuery] int? month, [FromQuery] int? year)
    {
        var transactions = _service.Get(bucket, month, year);
        if (transactions.Count > 0)
        {
            return Ok(transactions);
        }

        return BadRequest("No transactions found.");
    }
}
