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
        private readonly IUserService _userService;

        public ExpenseService(ExpenseTrackerContext db, ILogger<IExpenseService> logger, IUserService userService)
        {
            _db = db;
            _logger = logger;
            _userService = userService;
        }

        public async Task<Result<ResponseTransactionDTo?>> GetTransactionByID(int id)
        {
            var transaction = await _db.Transactions.FirstOrDefaultAsync(t => t.Id == id);
            _logger.LogInformation("GET request for transaction with ID: {id}", id);

            if (transaction == null)
            {
                return Result<ResponseTransactionDTo?>.NotFound();
            }

            var response = _mapper.TransactionToResponseTransaction(transaction);
            return Result<ResponseTransactionDTo?>.Success(response);
            


        }

        public async Task<Result<ResponseTransactionDTo?>> UpdateTransaction(string userId, int id, UpdateTransactionDto transaction)
        {
            var t = await _db.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == userId);

            if (t == null)
            {
                return Result<ResponseTransactionDTo?>.NotFound();
            }

            t.Amount = transaction.Amount;
            t.BucketId = transaction.BucketId;
            t.CreatedAt = transaction.CreatedAt;
            t.Description = transaction.Description;


            _db.Transactions.Update(t);
            await _db.SaveChangesAsync();
            var response = _mapper.TransactionToResponseTransaction(t);
            return Result<ResponseTransactionDTo?>.Success(
                response
            );
        }
        
        public async Task<Result<object>> GetTransactions(string? userId, int? month, int? year, int? bucket, int pageNumber = 1, int pageSize = 3)
        {
            if (userId == null) return Result<object>.Failure();
            
            // bucket query string = bucket ID
            var totalRecords = await _db.Transactions.Where(t => t.ApplicationUserId == userId).CountAsync();
            if (month.HasValue && year.HasValue && !bucket.HasValue)
            {
                var monthTransactions = await _db.Transactions
                    .Where(t => t.ApplicationUserId == userId && t.CreatedAt.Month == month && t.CreatedAt.Year == year)
                    .OrderByDescending(t => t.CreatedAt)
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
                .Where(t => t.ApplicationUserId == userId && t.BucketId == bucket)
                .OrderByDescending(t => t.CreatedAt)
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
                .Where(t => t.ApplicationUserId == userId && t.CreatedAt.Month == month && t.CreatedAt.Year == year && t.BucketId == bucket)
                .OrderByDescending(t => t.CreatedAt)
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
                .Where(t => t.ApplicationUserId == userId && t.CreatedAt.Month == month && t.CreatedAt.Year == year)
                .OrderByDescending(t => t.CreatedAt)
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
                    .Where(t => t.ApplicationUserId == userId && t.CreatedAt.Year == year)
                    .OrderByDescending(t => t.CreatedAt)
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
                .Where(t => t.ApplicationUserId == userId)
                .OrderByDescending(t => t.CreatedAt)
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

            public async Task<Result<ResponseTransactionDTo>> CreateTransaction(string userId, RequestTransactionDto transaction)
            {
                // mapp to Transaction object.
                var mappedTransaction = _mapper.TransactionDtoToRequestTransaction(transaction);
                mappedTransaction.ApplicationUserId = userId;
                
                // Salary bucket of the current logged in user.
                UserBuckets salary = await _db.UserBuckets.FirstAsync(ub => ub.BucketId == 1 && ub.ApplicationUserId == userId);
                
                // Bucket of the new created transaction.
                Bucket? transactionBucket = await _db.Buckets.FirstOrDefaultAsync(b => b.Id == mappedTransaction.BucketId);
                
                var userBuckets = await _db.UserBuckets.FirstAsync(ub =>
                    ub.ApplicationUserId == userId && ub.BucketId == mappedTransaction.BucketId); // bucketID: 2
                
                // Guard clauses, always start with null check first.
                if (transactionBucket == null || mappedTransaction.Amount <= 0)
                {
                    _logger.LogWarning("Failed to create new transaction, incorrect amount or bucket was provided.");
                    return Result<ResponseTransactionDTo>.Failure();
                }

                
                // 💡 GetValueOrDefault()
                // if the transaction is Income then we need to update the amount.
                // 1. The transaction is an Income transaction
                // 2. The transaction is an Expense transaction
                //      2.1 Increment the total of the expense bucket
                        //2.2 Decrement the Income bucket 
                        
                if (userBuckets.BucketId >= 0 && transactionBucket.Name != Buckets.Salary)
                {
                    userBuckets.Total+=mappedTransaction.Amount;
                    salary.Total -= mappedTransaction.Amount;
                }
                else
                {
                    userBuckets.Total += mappedTransaction.Amount;
                    
                }
                


                _db.Transactions.Add(mappedTransaction);
                _db.UserBuckets.Update(salary);
                await _db.SaveChangesAsync();
                
                var response = _mapper.TransactionToResponseTransaction(mappedTransaction);
                return Result<ResponseTransactionDTo>.Success(response);
                
            }

            public async Task<Result<bool>> DeleteTransaction(string userId, int transactionId)
            {
                var deletedTransaction = await _db.Transactions.FindAsync(transactionId);

                var transactionBucket = await _db.Buckets.FirstAsync(bucket => deletedTransaction != null && bucket.Id == deletedTransaction.BucketId);

                // var income = await _db.UserBuckets.FirstAsync(b => b. == Buckets.Salary);
                
                var userBuckets = await _db.UserBuckets.FirstAsync(ub =>
                    ub.BucketId == transactionBucket.Id && ub.ApplicationUserId == userId);
                
                var userIncome = await _db.UserBuckets.FirstAsync(ub =>
                    ub.BucketId == 1 && ub.ApplicationUserId == userId);
                
                


                if (deletedTransaction != null && deletedTransaction.Amount > 0)
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
                        userBuckets.Total -= deletedTransaction.Amount;
                        userIncome.Total += deletedTransaction.Amount;
                    }
                    else
                    {
                        userIncome.Total -= deletedTransaction.Amount;
                    }

                    await _db.SaveChangesAsync();
                    return Result<bool>.Success(true);
                }
            return Result<bool>.Failure();

        }


    }
}