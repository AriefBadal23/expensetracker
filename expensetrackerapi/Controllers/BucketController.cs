using System.Globalization;
using expensetrackerapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public IActionResult Get(Buckets? bucket, [FromQuery] int? month, [FromQuery] int? year)
    {

        if (!bucket.HasValue)
        {
            // TODO oplossen dat als ik geen bucket meegeef ik alles van een jaar te zien krijg.
            // nu zie ik het per maand..
        }

        // base query: all transactions for the bucket
        var query =
            from b in _db.Buckets
            join t in _db.Transactions on b.Id equals t.BucketId
            where b.Name == bucket
            select t;

        if (month.HasValue)
        {
            query = query.Where(x => x.Created_at.Month == month);
        }

        if (year.HasValue)
        {
            query = query.Where(x => x.Created_at.Year == year);

        }

        var grouped = (from t in query
                       group t by new { t.Created_at.Month, t.Created_at.Year } // composite keys
                      into grouping
                       select new
                       {
                           grouping.Key.Month,
                           grouping.Key.Year,
                           Total = grouping.Sum(x => x.Amount),
                           BucketId = grouping.Select(x => x.BucketId),

                       })
                       .OrderBy(x => x.Year)
                       .ThenBy(x => x.Month)
                       .ToList();

        var labels = grouped.Select(x => year.HasValue ? CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month) : $"{x.Year}-{x.Month:00}").ToArray();
        var totals = grouped.Select(x => x.Total).ToArray();
        var bucketName = grouped.Select(x => x.BucketId).First();

        return Ok(new
        {
            Labels = labels,
            Bucket = bucketName.First(),
            Totals = totals,
            Month = month,
            Year = year
        });
    }
}
