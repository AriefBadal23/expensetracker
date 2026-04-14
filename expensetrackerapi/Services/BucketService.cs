using expensetrackerapi.Contracts;
using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using expensetrackerapi.Results;
using Microsoft.EntityFrameworkCore;

namespace expensetrackerapi.Services;


public class BucketService : IBucketService
{
    private readonly ExpenseTrackerContext _db;

    public BucketService(ExpenseTrackerContext context)
    {
        _db = context;
    }

    public async Task<Result<List<Bucket>>> GetBuckets()
    {
        return Result<List<Bucket>>.Success(
            await _db.Buckets.OrderBy(b => b.Name).ToListAsync()
        );
    }

    public async Task<Result<List<UserBucketResponseDto>>> GetBucketsByUserId(string? userId)
    {
        var buckets = from bucket in _db.Buckets
            join userbucket in _db.UserBuckets on bucket.Id equals userbucket.BucketId into Userbucketgroup
            
            from userbucket in Userbucketgroup
            where userbucket.ApplicationUserId == userId
            select new UserBucketResponseDto
            {
                Bucket = bucket,
                BucketTotal = userbucket.Total
            };

        return Result<List<UserBucketResponseDto>>.Success(
            await buckets.ToListAsync());
    }


    public async Task<Result<BucketResponseDto>> GetSummary(string userId, int month, int year)
    {
        if (month == 0 || year == 0)
            return
                Result<BucketResponseDto>.Success(new BucketResponseDto
                {
                    Buckets = new List<BucketTransaction>()
                });

        var query = await (from buck in _db.Buckets
                           join transaction in _db.Transactions on buck.Id equals transaction.BucketId
                               into bucketTransactions

                           let userTransactions = bucketTransactions
                               .Where(t => t.CreatedAt.Month == month && t.CreatedAt.Year == year && t.ApplicationUserId == userId)
                           

                           let monthBucketTotal = userTransactions.Sum(x => x.Amount)

                           select
                                   new BucketTransaction(buck.Id, buck.Name,buck.Type, monthBucketTotal, userTransactions.ToArray())
            ).ToListAsync();


        // Make use of the query but change the return type so it matches the required output for the front-end.
        return Result<BucketResponseDto>.Success(
            new BucketResponseDto
            {
                Month = month,
                Year = year,
                Buckets = query,
                TotalExpenses = query.Where(x => x.BucketName != Buckets.Salary).Sum(x => x.BucketExpenseTotal),
                TotalIncome = query.Where(x => x.BucketName == Buckets.Salary).Sum(x => x.BucketExpenseTotal),
            });

    }

}
