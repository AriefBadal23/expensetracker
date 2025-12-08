using Microsoft.AspNetCore.Mvc;

namespace expensetrackerapi.Controllers;

[ApiController]
[Route("")]
public class BucketsController : ControllerBase
{
    private readonly Bucket[] BucketsArray = [
        new Bucket {Id=1,Total=0, icon="💰", Name= Buckets.Salary},
        new Bucket {Id=2,Total=0, icon="🛒", Name=Buckets.Shopping},
        new Bucket { Id = 3,  Total = 0, icon = "🏪", Name = Buckets.Groceries }
   ];

    private readonly ILogger<BucketsController> _logger;

    public BucketsController(ILogger<BucketsController> logger)
    {
        // constructor DI
        _logger = logger;
    }

    [HttpGet(Name = "Buckets")]
    public IActionResult Get()// Action method
    {
        if (BucketsArray is null) return NotFound();
        return Ok(BucketsArray);
    }
}
