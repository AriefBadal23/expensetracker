using expensetrackerapi.Controllers;
using expensetrackerapi.Models;


namespace expensetrackerapi.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ExpenseTrackerContext _db;

        public ExpenseService(ExpenseTrackerContext db)
        {
            _db = db;
        }

        public Transaction? GetTransactionByID(int id)
        {
            var transaction = _db.Transactions.FirstOrDefault(_ => _.Id == id);
            return transaction ?? null;
        }

        public Transaction? UpdateTransaction(Transaction transaction)
        {

            var t = _db.Transactions.Find(transaction.Id);
            
            if (t == null)
            {
                return null;
            }
            
            t.Id = transaction.Id;
            t.Amount = transaction.Amount;
            t.BucketId = transaction.BucketId;
            t.Created_at = transaction.Created_at;
            t.Description = transaction.Description;
            t.IsIncome = transaction.IsIncome;
            
            _db.Transactions.Update(t);
            _db.SaveChanges();
            
            return  t;
        }

        public object GetTransactions(int? month, int? year, int? bucket, int pageNumber = 1, int pageSize = 3)
        {
            // bucket query string = bucket ID
            var totalRecords = _db.Transactions.Count();
            if (month.HasValue && year.HasValue && !bucket.HasValue)
            {
                var month_transactions = _db.Transactions
                    .Where(_ => _.Created_at.Month == month && _.Created_at.Year == year)
                    .OrderBy(_ => _.Created_at.Year).ThenBy(_ => _.Created_at.Month)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize).ToList();

                // TODO: ResponseDTO
                return new
                {
                    Total = month_transactions.Count,
                    Transactions = month_transactions
                };
            }
            else if (!month.HasValue && bucket.HasValue)
            {
                var bucket_transactions = _db.Transactions
                .Where(_ => _.BucketId == bucket)

                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(_ => _.BucketId).ToList();

                // TODO: ResponseDTO
                return new
                {
                    Total = totalRecords,
                    Transactions = bucket_transactions
                };
            }


            else if (month.HasValue && year.HasValue && bucket.HasValue)
            {
                var month_transactions = _db.Transactions
                .Where(_ => _.Created_at.Month == month && _.Created_at.Year == year && _.BucketId == bucket)
                .OrderBy(_ => _.Created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
                return new
                {
                    Total = month_transactions.Count,
                    Transactions = month_transactions
                };
            }
            else if (month.HasValue && year.HasValue)
            {
                var month_transactions = _db.Transactions
                .Where(_ => _.Created_at.Month == month && _.Created_at.Year == year)
                .OrderBy(_ => _.Created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                // TODO: ResponseDTO
                return new
                {
                    Total = month_transactions.Count,
                    Transactions = month_transactions
                };
            }


            // No month is provided.
            var transactions = _db.Transactions
                .OrderByDescending(_ => _.Created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();
               
            return new
            {
                transactions,
                Total = totalRecords,

            };

        }

        public bool CreateTransaction(CreateTransactionDto dto)
        {
            // We check first if the bucket exists at all
            var bucketExists = _db.Buckets.Find(dto.BucketId);

            // Make sure the input of the client is not negative and the bucket exists
            if (dto.Amount > 0 && bucketExists != null)
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
                return true;
            }
            return false;
        }

        public bool DeleteTransaction(int transactionID)
        {
            Transaction? t = _db.Transactions.FirstOrDefault(_ => _.Id == transactionID);
            var transactionBucket = _db.Buckets.FirstOrDefault(_ => t != null && _.Id == t.BucketId);
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
                return true;
            }
            return false;

        }


    }
}