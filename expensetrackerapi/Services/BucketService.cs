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

    public async Task<Result<BucketResponseDto>> GetSummary(int month, int year)
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

                           let BuckTransactionsResult = bucketTransactions
                               .Where(t => t.Created_at.Month == month && t.Created_at.Year == year)

                           let monthBucketTotal = BuckTransactionsResult.Sum(x => x.Amount)

                           select
                                   new BucketTransaction(buck.Id, buck.Name,buck.Type, monthBucketTotal, BuckTransactionsResult.ToArray())
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
