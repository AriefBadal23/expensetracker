using expensetrackerapi.Contracts;
using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using expensetrackerapi.Results;
using Microsoft.EntityFrameworkCore;

namespace expensetrackerapi.Services;


public class BucketService : IBucketService
{
    private readonly ExpenseTrackerContext _db;
    private readonly ILogger<BucketService> _logger;

    public BucketService(ExpenseTrackerContext context, ILogger<BucketService> logger)
    {
        _db = context;
        _logger = logger;
    }


    public async Task<Result<List<UserBucketResponseDto>>> GetBucketsByUserId(string? userId)
    {
        var userDoesExists = await _db.Users.AnyAsync(u => u.Id == userId);
        if (!userDoesExists)
        {
            _logger.LogWarning("Failed to retrieve buckets due invalid userId for userId: {UserId}", userId);
            return Result<List<UserBucketResponseDto>>.Failure();
        }
        
        var buckets = from bucket in _db.Buckets
                      join userbucket in _db.UserBuckets on bucket.Id equals userbucket.BucketId into Userbucketgroup

                      from userbucket in Userbucketgroup
                      where userbucket.ApplicationUserId == userId
                      select new UserBucketResponseDto
                      {
                          Bucket = bucket,
                          BucketTotal = userbucket.Total
                      };

        _logger.LogInformation("Successfully retrieved buckets by userId: {UserId}", userId);
        return Result<List<UserBucketResponseDto>>.Success(
            await buckets.ToListAsync());
    }

    public async Task<Result<UserBucketResponseDto>> CreateBucket(string? userId, BucketRequestDto bucket)
    {
        var userDoesExists = await _db.Users.AnyAsync(u => u.Id == userId);
        if (!userDoesExists)
        {
            return Result<UserBucketResponseDto>.Failure();
        }
        
        
        Bucket newBucket = new Bucket
        {
            Icon = bucket.Icon,
            Name= bucket.Name,
            Type = bucket.Type
        };



        
        await _db.Buckets.AddAsync(newBucket);
        await _db.SaveChangesAsync();
        
        var UserBucket = new UserBuckets { ApplicationUserId = userId, BucketId = _db.Buckets.First(b => b.Name == bucket.Name).Id };
        
        await _db.UserBuckets.AddAsync(UserBucket);
        await _db.SaveChangesAsync();

        var userBucketTotal =  _db.UserBuckets.First(ub => ub.ApplicationUserId == userId).Total;
        
        return Result<UserBucketResponseDto>.Success(new UserBucketResponseDto
        {
            Bucket = newBucket,
            BucketTotal = userBucketTotal
        });

    }
    
    public async Task<Result<BucketSummaryResponseDto>> GetSummary(string userId, int month, int year)
    {
        var userDoesExists = await _db.Users.AnyAsync(u => u.Id == userId);

        if (!userDoesExists)
        {
            _logger.LogWarning("Failed to retrieve transactions summary due invalid userId for userId: {UserId}", userId);
            return Result<BucketSummaryResponseDto>.Failure();
        }

        if (month == 0 || year == 0)
        {
            _logger.LogInformation("Successfully retrieved Bucket Transactions summary by userId for {UserId} without month and year", userId);
            return
                Result<BucketSummaryResponseDto>.Success(new BucketSummaryResponseDto
                {
                    Buckets = new List<BucketTransaction>()
                });
        }

        var query = await (from buck in _db.Buckets
                           join transaction in _db.Transactions on buck.Id equals transaction.BucketId
                               into bucketTransactions

                           let userTransactions = bucketTransactions
                               .Where(t => t.CreatedAt.Month == month && t.CreatedAt.Year == year && t.ApplicationUserId == userId)


                           let monthBucketTotal = userTransactions.Sum(x => x.Amount)

                           select
                                   new BucketTransaction(buck.Id, buck.Name, buck.Type, monthBucketTotal, userTransactions.ToArray())
            ).ToListAsync();


        // Make use of the query but change the return type so it matches the required output for the front-end.
        _logger.LogInformation("Successfully retrieved Bucket Transactions summary by userId for {UserId}.", userId);

        return Result<BucketSummaryResponseDto>.Success(
            new BucketSummaryResponseDto
            {
                Month = month,
                Year = year,
                Buckets = query,
                TotalExpenses = query.Where(x => x.BucketName != nameof(Buckets.Salary)).Sum(x => x.BucketExpenseTotal),
                TotalIncome = query.Where(x => x.BucketName == nameof(Buckets.Salary)).Sum(x => x.BucketExpenseTotal),
            });

    }

}
