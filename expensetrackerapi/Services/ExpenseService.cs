using expensetrackerapi.Contracts;
using expensetrackerapi.DTO;
using expensetrackerapi.Mapper;
using expensetrackerapi.Models;
using expensetrackerapi.Results;
using Microsoft.EntityFrameworkCore;


namespace expensetrackerapi.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ExpenseTrackerContext _db;
        private readonly TransactionMapper _mapper = new();
        private readonly ILogger<IExpenseService> _logger;

        public ExpenseService(ExpenseTrackerContext db, ILogger<IExpenseService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Result<ResponseTransactionDTo?>> GetTransactionByID(int id)
        {
            var transaction = await _db.Transactions.FirstOrDefaultAsync(t => t.Id == id);
            _logger.LogInformation("GET request for transaction with ID: {id}", id);

            return Result<ResponseTransactionDTo?>.Success(
                _mapper.TransactionToResponseTransaction(transaction)
                );

        }

        public async Task<Result<ResponseTransactionDTo?>> UpdateTransaction(Transaction transaction)
        {

            if (transaction.Id <= 0) return Result<ResponseTransactionDTo?>.NotFound();

            var t = await _db.Transactions.FindAsync(transaction.Id);

            if (t == null)
            {
                return Result<ResponseTransactionDTo?>.NotFound();
            }

            t.Amount = transaction.Amount;
            t.BucketId = transaction.BucketId;
            t.Created_at = transaction.Created_at;
            t.Description = transaction.Description;


            _db.Transactions.Update(t);
            await _db.SaveChangesAsync();
            var response = _mapper.TransactionToResponseTransaction(t);
            return Result<ResponseTransactionDTo?>.Success(
                response
            );
        }

        public async Task<Result<object>> GetTransactions(int? month, int? year, int? bucket, int pageNumber = 1, int pageSize = 3)
        {
            // bucket query string = bucket ID
            var totalRecords = await _db.Transactions.CountAsync();
            if (month.HasValue && year.HasValue && !bucket.HasValue)
            {
                var monthTransactions = await _db.Transactions
                    .Where(t => t.Created_at.Month == month && t.Created_at.Year == year)
                    .OrderByDescending(t => t.Created_at)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize).ToListAsync();

                // TODO: ResponseDTO
                return Result<object>.Success(new
                {
                    Total = monthTransactions.Count,
                    Transactions = monthTransactions
                })
               ;
            }
            else if (!month.HasValue && bucket.HasValue)
            {
                var bucketTransactions = await _db.Transactions
                .Where(b => b.BucketId == bucket)
                .OrderByDescending(t => t.Created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

                // TODO: ResponseDTO
                return Result<object>.Success(
                new
                {
                    Total = totalRecords,
                    Transactions = bucketTransactions
                });
            }


            else if (month.HasValue && year.HasValue && bucket.HasValue)
            {
                var monthTransactions = await _db.Transactions
                .Where(t => t.Created_at.Month == month && t.Created_at.Year == year && t.BucketId == bucket)
                .OrderByDescending(t => t.Created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
                return Result<object>.Success(new
                {
                    Total = monthTransactions.Count,
                    Transactions = monthTransactions
                });
            }
            else if (month.HasValue && year.HasValue)
            {
                var monthTransactions = await _db.Transactions
                .Where(t => t.Created_at.Month == month && t.Created_at.Year == year)
                .OrderByDescending(t => t.Created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

                // TODO: ResponseDTO
                return Result<object>.Success(
                    new
                    {
                        Total = monthTransactions.Count,
                        Transactions = monthTransactions
                    }
                );
            }

            else if (year.HasValue)
            {
                var monthTransactions = await _db.Transactions
                    .Where(t => t.Created_at.Year == year)
                    .OrderByDescending(t => t.Created_at)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // TODO: ResponseDTO
                return Result<object>.Success(
                    new
                    {
                        Total = monthTransactions.Count,
                        Transactions = monthTransactions
                    }
                    );
            }


            // No month is provided.
            var transactions = await _db.Transactions
                .OrderByDescending(t => t.Created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return Result<object>.Success(
                new
                {
                    transactions,
                    Total = totalRecords,

                }
            );

        }

        public async Task<Result<ResponseTransactionDTo?>> CreateTransaction(RequestTransactionDto transaction)
        {
            var mappedTransaction = _mapper.TransactionDtoToRequestTransaction(transaction);

            Bucket salary = await _db.Buckets.FirstAsync(x => x.Name == Buckets.Salary);
            Bucket? transactionBucket = await _db.Buckets.FirstOrDefaultAsync(b => b.Id == mappedTransaction.BucketId);

            // Guard clauses, always start with null check first.
            if (transactionBucket == null || mappedTransaction.Amount <= 0)
            {
                _logger.LogWarning("Failed to create new transaction, incorrect amount or bucket was provided.");
                return Result<ResponseTransactionDTo?>.Failure();
            }

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
            await _db.SaveChangesAsync();
            var response = _mapper.TransactionToResponseTransaction(mappedTransaction);
            return Result<ResponseTransactionDTo?>.Success(response);
            
        }

        public async Task<Result<bool>> DeleteTransaction(int transactionId)
        {
            var deletedTransaction = await _db.Transactions.FindAsync(transactionId);

            var transactionBucket = await _db.Buckets.FirstOrDefaultAsync(bucket => deletedTransaction != null && bucket.Id == deletedTransaction.BucketId);

            Bucket income = await _db.Buckets.FirstAsync(b => b.Name == Buckets.Salary);


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

                await _db.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            return Result<bool>.Failure();

        }


    }
}