using expensetrackerapi.Models;
using Microsoft.AspNetCore.Mvc;
using expensetrackerapi.Services;

namespace expensetrackerapi.Controllers
{
    [Route("api/v1/[controller]")]
    public class Transactions : Controller
    {
        private readonly ILogger<Transactions> _logger;
        private readonly ExpenseTrackerContext _db;

        private readonly IExpenseService _expenseService;

        public Transactions(ILogger<Transactions> logger, ExpenseTrackerContext context, IExpenseService service)
        {
            _logger = logger;
            _db = context;
            _expenseService = service;
        }


        [HttpGet]
        public ActionResult<IExpenseService> Get(
            [FromQuery] int? month, int? year,
            [FromQuery] int? bucket,
            int pageNumber = 1, int pageSize = 3)
        {
            var transactions = _expenseService.GetTransactions(month, year, bucket, pageNumber, pageSize);
            if (transactions is null)
            {
                return NotFound();
            }
            return Ok(transactions);

        }

        [HttpPost]
        public ActionResult Post([FromBody] CreateTransactionDto dto)
        {
            // We check first if the bucket exists at all
            bool BucketExists = _db.Buckets.Any(x => x.Id == dto.BucketId);

            // Make sure the input of the client is not negative and the bucket exists
            if (dto.Amount > 0 && BucketExists)
            {
                // We get the salary bucket object.
                Bucket Salary = _db.Buckets.First(x => x.Name == Buckets.Salary);

                // Its necassary to update the Salary bucket total if it is an income otherwise it will substract from it.
                Salary.Total = Salary.Total > 0 && dto.IsIncome == false ? Salary.Total - dto.Amount : Salary.Total + dto.Amount;

                // Make sure we update the other bucket total amounts
                Bucket TransactionBucket = _db.Buckets.First(x => x.Id == dto.BucketId);
                TransactionBucket.Total = dto.Amount > 0 && dto.IsIncome == false ? TransactionBucket.Total + dto.Amount : TransactionBucket.Total;



                _db.Buckets.UpdateRange([Salary, TransactionBucket]);

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

        [HttpDelete("{transactionID}")]
        public ActionResult Delete(int transactionID)
        {
            Transaction? t = _db.Transactions.FirstOrDefault(_ => _.Id == transactionID);
            var transactionBucket = _db.Buckets.FirstOrDefault(_ => _.Id == t.BucketId);
            Bucket Income = _db.Buckets.Where(_ => _.Name == Buckets.Salary).First();


            if (t != null && transactionBucket != null)
            {
                _db.Transactions.Remove(t);
                // Je kijkt of the transaction een income of expense is. 
                // Daarop update je de Income total.
                // -----
                // Is the transaction not an income
                // then add it back to the income and decrease the bucket amount.
                // otherwise decrease it from the income and decrease it from the Income as well.

                if (t.IsIncome is false)
                {
                    transactionBucket.Total -= t.Amount;
                    Income.Total += t.Amount;
                }
                else
                {
                    Income.Total -= t.Amount;
                }



                _db.SaveChanges();
                return NoContent();
            }
            return NotFound();
        }
    }
}