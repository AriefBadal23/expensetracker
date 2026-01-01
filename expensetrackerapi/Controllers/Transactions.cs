using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using expensetrackerapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
                .Where(_ => _.Created_at.Date.Month == month)

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
                .Where(_ => _.Created_at.Date.Month == month && _.BucketId == bucket)
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
        public ActionResult Post([FromBody] Transaction transaction)
        {
            if (transaction != null)
            {
                if (transaction.isExpense == true)
                {
                    var Salary = _db.Buckets.First(x => x.Name == Buckets.Salary && transaction.BucketId != x.Id);
                    Salary.Total = Salary.Total > 0 ? Salary.Total - transaction.Amount : 0;
                    _db.Buckets.Update(Salary);
                }
                _db.Transactions.Add(transaction);
                var bucket = _db.Buckets.First(b => b.Id == transaction.BucketId);
                bucket.Total = transaction.Amount + bucket.Total;

                _db.Buckets.Update(bucket);
                _db.SaveChanges();

                return Ok();
            }

            return BadRequest("Invalid Transaction.");

        }
    }
}