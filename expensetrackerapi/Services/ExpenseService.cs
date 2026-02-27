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
                .OrderByDescending(t => t.Created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();

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
            
            Bucket salary = _db.Buckets.First(x => x.Name == Buckets.Salary);
            Bucket? transactionBucket = _db.Buckets.FirstOrDefault(b => b.Id == mappedTransaction.BucketId);

            // Guard clauses, always start with null check first.
            if (transactionBucket == null || mappedTransaction.Amount <= 0) return new ResponseTransactionDTo();
            
            if (transactionBucket.Type == BucketTypes.Income && mappedTransaction.BucketId == 1)
            {
                salary.Total += mappedTransaction.Amount;
            }
            else
            {
                salary.Total -= mappedTransaction.Amount;
                    
            }

            if (transactionBucket.Total >= 0 && transactionBucket.Name != Buckets.Salary)
            {
                transactionBucket.Total += mappedTransaction.Amount;
            }
                
            _db.Transactions.Add(mappedTransaction);
            _db.Buckets.UpdateRange([salary, transactionBucket]);
            _db.SaveChanges();
            var response = _mapper.TransactionToResponseTransaction(mappedTransaction);
            return response;
        }

        public bool DeleteTransaction(int transactionId)
        {
            var deletedTransaction = _db.Transactions.Find(transactionId);
            
            var transactionBucket = _db.Buckets.FirstOrDefault(bucket => deletedTransaction != null && bucket.Id == deletedTransaction.BucketId);
            
            Bucket income = _db.Buckets.First(b => b.Name == Buckets.Salary);


            if (deletedTransaction != null && deletedTransaction.Amount > 0 && transactionBucket != null)
            {
                _db.Transactions.Remove(deletedTransaction);
                // Je kijkt of the transaction een income of expense is. 
                // Daarop update je de Income total.
                // -----
                // Is the transaction not an income
                // then add it back to the income and decrease the bucket amount.
                // otherwise decrease it from the income and decrease it from the Income as well.

                if (transactionBucket.Type == BucketTypes.Expense)
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