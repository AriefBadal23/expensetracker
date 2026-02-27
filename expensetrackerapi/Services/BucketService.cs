using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace expensetrackerapi.Services;


public class BucketService : IBucketService
{
    private readonly ExpenseTrackerContext _db;

    public BucketService(ExpenseTrackerContext context)
    {
        _db = context;
    }

    public List<Bucket> GetBuckets()
    {
        return _db.Buckets.OrderBy(b => b.Name).ToList();
    }

    public BucketResponseDto GetSummary(int month, int year)
    {
        if (month == 0 || year == 0) return 
            new BucketResponseDto
            {
                Buckets = new List<BucketTransaction>()
            };

        var query = (from buck in _db.Buckets
            join transaction in _db.Transactions on buck.Id equals transaction.BucketId
                into bucketTransactions
            
            let BuckTransactionsResult = bucketTransactions
                .Where(t => t.Created_at.Month == month && t.Created_at.Year == year)
            
            let monthBucketTotal = BuckTransactionsResult.Sum(x => x.Amount)
                
            select 
                    new BucketTransaction(buck.Id, buck.Name,monthBucketTotal, BuckTransactionsResult.ToArray())
            ).ToList();

        
        // Make use of the query but change the return type so it matches the required output for the front-end.
        return new BucketResponseDto
        {
            Month = month,
            Year = year,
            Buckets = query,
            TotalExpenses = query.Where(x => x.BucketName != Buckets.Salary).Sum(x => x.BucketExpenseTotal),
            TotalIncome = query.Where(x => x.BucketName == Buckets.Salary).Sum(x => x.BucketExpenseTotal),
        };

    }

        }
