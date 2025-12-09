using expensetrackerapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace expensetrackerapi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BucketsController : ControllerBase
{
    private readonly ILogger<BucketsController> _logger;
    private readonly ExpenseTrackerContext _db;

    public BucketsController(ILogger<BucketsController> logger, ExpenseTrackerContext context)
    {
        // constructor DI 
        _logger = logger;
        _db = context;
    }


    [HttpGet("{bucket?}")]
    public IActionResult Get(string? bucket)// Action method
    {

        if (!string.IsNullOrEmpty(bucket) && Enum.TryParse<Buckets>(bucket, ignoreCase: true, out var bucketEnum))
        {
            var buckets = from b in _db.Buckets
                          join transaction in _db.Transactions on b.Id equals transaction.BucketId
                          where b.Name == bucketEnum
                          select new { b, transaction };

            if (buckets != null)
            {
                return Ok(buckets);
            }
            else
            {
                return NotFound($"No bucket found for {bucket}.");
            }
        }
        return Ok(_db.Buckets);
    }

};
