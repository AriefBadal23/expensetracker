using expensetrackerapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace expensetrackerapi.Controllers
{
    [Route("api/v1/[controller]")]
    public class Transactions : Controller
    {
        private readonly ILogger<Transactions> _logger;
        private readonly ExpenseTrackerContext _db;

        public Transactions(ILogger<Transactions> logger, ExpenseTrackerContext context)
        {
            _logger = logger;
            _db = context;
        }


        [HttpGet]
        public ActionResult Get([FromQuery] int? month, int? year, [FromQuery] int? bucket, int pageNumber = 1, int pageSize = 3)
        {
            // bucket query string = bucket ID
            var totalRecords = _db.Transactions.Count();
            if (month.HasValue && year.HasValue && !bucket.HasValue)
            {
                var month_transactions = _db.Transactions
                .Where(_ => _.Created_at.Month == month && _.Created_at.Year == year)

                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(_ => _.Created_at).ToList();

                return Ok(new
                {
                    Total = month_transactions.Count,
                    Transactions = month_transactions
                });
            }
            else if (!month.HasValue && bucket.HasValue)
            {
                var bucket_transactions = _db.Transactions
                .Where(_ => _.BucketId == bucket)

                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(_ => _.BucketId).ToList();
                return Ok(new
                {
                    Total = totalRecords,
                    Transactions = bucket_transactions
                });
            }


            else if (month.HasValue && year.HasValue && bucket.HasValue)
            {
                var month_transactions = _db.Transactions
                .Where(_ => _.Created_at.Month == month && _.Created_at.Year == year && _.BucketId == bucket)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(_ => _.Created_at).ToList();
                return Ok(new
                {
                    Total = month_transactions.Count,
                    Transactions = month_transactions
                });
            }
            else if (month.HasValue && year.HasValue)
            {
                var month_transactions = _db.Transactions
                .Where(_ => _.Created_at.Month == month && _.Created_at.Year == year)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(_ => _.Created_at).ToList();
                return Ok(new
                {
                    Total = month_transactions.Count,
                    Transactions = month_transactions
                });
            }


            // No month is provided.
            var transactions = _db.Transactions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(_ => _.Created_at).ToList();
            if (transactions == null) return NotFound("No transactions found.");
            return Ok(new
            {
                transactions,
                Total = totalRecords,

            });


        }

        [HttpPost]
        public ActionResult Post([FromBody] CreateTransactionDto dto)
        {
            bool BucketExists = _db.Buckets.Any(x => x.Id == dto.BucketId);

            if (dto.Amount > 0 && BucketExists)
            {
                Bucket Salary = _db.Buckets.First(x => x.Name == Buckets.Salary);
                Salary.Total = Salary.Total > 0 && dto.IsIncome == false ? Salary.Total - dto.Amount : Salary.Total + dto.Amount;



                _db.Buckets.Update(Salary);

                var transaction = new Transaction
                {
                    UserId = 1,
                    BucketId = dto.BucketId,
                    Description = dto.Description,
                    Amount = dto.Amount,
                    Created_at = dto.CreatedAt,
                    IsIncome = dto.IsIncome
                };
                _db.Transactions.Add(transaction);
                _db.SaveChanges();
            }

            return Created();

        }
    }
}