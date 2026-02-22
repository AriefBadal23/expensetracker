using expensetrackerapi.DTO;
using expensetrackerapi.Mapper;
using expensetrackerapi.Models;


namespace expensetrackerapi.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ExpenseTrackerContext _db;
        private TransactionMapper _mapper = new();

        public ExpenseService(ExpenseTrackerContext db)
        {
            _db = db;
        }

        public ResponseTransactionDTo? GetTransactionByID(int id)
        {
            var transaction = _db.Transactions.FirstOrDefault(t => t.Id == id);
            return _mapper.TransactionToResponseTransaction(transaction);

        }

        public ResponseTransactionDTo? UpdateTransaction(Transaction transaction)
        {
            
            if (transaction.Id <= 0 ) return null;
            
            var t = _db.Transactions.Find(transaction.Id);
            
            if (t == null)
            {
                return null;
            }
            
            t.Amount = transaction.Amount;
            t.BucketId = transaction.BucketId;
            t.Created_at = transaction.Created_at;
            t.Description = transaction.Description;
            t.IsIncome = transaction.IsIncome;
            
            _db.Transactions.Update(t);
            _db.SaveChanges();
            var response = _mapper.TransactionToResponseTransaction(t);
            return  response;
        }

        public object GetTransactions(int? month, int? year, int? bucket, int pageNumber = 1, int pageSize = 3)
        {
            // bucket query string = bucket ID
            var totalRecords = _db.Transactions.Count();
            if (month.HasValue && year.HasValue && !bucket.HasValue)
            {
                var monthTransactions = _db.Transactions
                    .Where(t => t.Created_at.Month == month && t.Created_at.Year == year)
                    .OrderByDescending(t => t.Created_at)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize).ToList();

                // TODO: ResponseDTO
                return new
                {
                    Total = monthTransactions.Count,
                    Transactions = monthTransactions
                };
            }
            else if (!month.HasValue && bucket.HasValue)
            {
                var bucketTransactions = _db.Transactions
                .Where(b=> b.BucketId == bucket)

                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(t => t.Created_at).ToList();

                // TODO: ResponseDTO
                return new
                {
                    Total = totalRecords,
                    Transactions = bucketTransactions
                };
            }


            else if (month.HasValue && year.HasValue && bucket.HasValue)
            {
                var monthTransactions = _db.Transactions
                .Where(t=> t.Created_at.Month == month && t.Created_at.Year == year && t.BucketId == bucket)
                .OrderByDescending(t => t.Created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
                return new
                {
                    Total = monthTransactions.Count,
                    Transactions = monthTransactions
                };
            }
            else if (month.HasValue && year.HasValue)
            {
                var monthTransactions = _db.Transactions
                .Where(t => t.Created_at.Month == month && t.Created_at.Year == year)
                .OrderByDescending(t => t.Created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                // TODO: ResponseDTO
                return new
                {
                    Total = monthTransactions.Count,
                    Transactions = monthTransactions
                };
            }
            
            else if (year.HasValue)
            {
                var monthTransactions = _db.Transactions
                    .Where(t => t.Created_at.Year == year)
                    .OrderByDescending(t => t.Created_at)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // TODO: ResponseDTO
                return new
                {
                    Total = monthTransactions.Count,
                    Transactions = monthTransactions
                };
            }


            // No month is provided.
            var transactions = _db.Transactions
                .OrderByDescending(t => t.Created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();
               
            return new
            {
                transactions,
                Total = totalRecords,

            };

        }

        public ResponseTransactionDTo? CreateTransaction(RequestTransactionDto transaction)
        {
            var mappedTransaction = _mapper.TransactionDtoToRequestTransaction(transaction);
            // We check first if the bucket exists at all
            var bucketExists = _db.Buckets.Find(mappedTransaction.BucketId);
            // Make sure the input of the client is not negative and the bucket exists
            if (mappedTransaction.Amount > 0 && bucketExists != null)
            {
                // We get the salary bucket object.
                Bucket salary = _db.Buckets.First(x => x.Name == Buckets.Salary);

                // Its necassary to update the Salary bucket total if it is an income otherwise it will substract from it.
                salary.Total = salary.Total > 0 && mappedTransaction.IsIncome == false ? salary.Total - mappedTransaction.Amount : salary.Total + mappedTransaction.Amount;

                // Make sure we update the other bucket total amounts
                Bucket transactionBucket = _db.Buckets.First(x => x.Id == mappedTransaction.BucketId);
                transactionBucket.Total = mappedTransaction.Amount > 0 && mappedTransaction.IsIncome == false ? transactionBucket.Total + mappedTransaction.Amount : transactionBucket.Total;



                _db.Buckets.UpdateRange([salary, transactionBucket]);
                _db.Transactions.Add(mappedTransaction);
                
                
                _db.SaveChanges();
                var response = _mapper.TransactionToResponseTransaction(mappedTransaction);
                
                return response;


            }
            return new ResponseTransactionDTo();
        }

        public bool DeleteTransaction(int transactionId)
        {
            var deletedTransaction = _db.Transactions.FirstOrDefault(t => t.Id == transactionId);
            var transactionBucket = _db.Buckets.FirstOrDefault(bucket => deletedTransaction != null && bucket.Id == deletedTransaction.BucketId);
            Bucket income = _db.Buckets.First(b => b.Name == Buckets.Salary);


            if (deletedTransaction != null && transactionBucket != null)
            {
                _db.Transactions.Remove(deletedTransaction);
                // Je kijkt of the transaction een income of expense is. 
                // Daarop update je de Income total.
                // -----
                // Is the transaction not an income
                // then add it back to the income and decrease the bucket amount.
                // otherwise decrease it from the income and decrease it from the Income as well.

                if (deletedTransaction.IsIncome is false)
                {
                    transactionBucket.Total -= deletedTransaction.Amount;
                    income.Total += deletedTransaction.Amount;
                }
                else
                {
                    income.Total -= deletedTransaction.Amount;
                }



                _db.SaveChanges();
                return true;
            }
            return false;

        }


    }
}