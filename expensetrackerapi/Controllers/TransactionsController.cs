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
        public ActionResult Get([FromQuery] int? month, [FromQuery] int? bucket, int pageNumber = 1, int pageSize = 3)
        {
            // bucket query string = bucket ID
            var totalRecords = _db.Transactions.Count();
            if (month.HasValue && !bucket.HasValue)
            {
                var month_transactions = _db.Transactions
                .Where(_ => _.Created_at.Month == month)

                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(_ => _.Created_at).ToList();

                return Ok(new
                {
                    Total = totalRecords,
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


            else if (month.HasValue && bucket.HasValue)
            {
                var month_transactions = _db.Transactions
                .Where(_ => _.Created_at.Month == month && _.BucketId == bucket)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(_ => _.Created_at).ToList();
                return Ok(new
                {
                    Total = totalRecords,
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
                int SalaryAmount = Salary.Total;
                SalaryAmount += dto.Amount;


                _db.Buckets.Update(Salary);

                var transaction = new Transaction
                {
                    UserId = 1,
                    BucketId = dto.BucketId,
                    Description = dto.Description,
                    Amount = dto.Amount,
                    Created_at = dto.CreatedAt,
                    isExpense = dto.IsExpense
                };
                _db.Transactions.Add(transaction);
                _db.SaveChanges();
            }

            return Created();

        }
    }
}