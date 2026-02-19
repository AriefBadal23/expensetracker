using expensetrackerapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace expensetrackerapi.Services;

public interface IBucketService
{
    public SummaryDTO GetSummary(int month, int year);
    public List<Bucket> GetBuckets(Buckets bucket);
}


public record BucketTransactionsDTO(int BucketId, Buckets BucketName, int BucketExpenseTotal, Transaction[] Transactions);

public record SummaryDTO
{
    public int Month { get; set; }
    public int Year { get; set; }
    public List<BucketTransactionsDTO> Buckets { get; set; }

    public int TotalIncome { get; set; }
    public int TotalExpenses { get; set; }

}

public class BucketService : IBucketService
{
    private readonly ExpenseTrackerContext _db;

    public BucketService(ExpenseTrackerContext context)
    {
        _db = context;
    }

    public List<Bucket> GetBuckets(Buckets bucket)
    {
        return _db.Buckets.ToList();
    }

    public SummaryDTO GetSummary(int month, int year)
    {
        if (month == 0 || year == 0) return new SummaryDTO();

        var query = (from buck in _db.Buckets
            join transaction in _db.Transactions on buck.Id equals transaction.BucketId
                into bucketTransactions
            
            let BuckTransactionsResult = bucketTransactions
                .Where(t => t.Created_at.Month == month && t.Created_at.Year == year)
            
            let MonthBucketTotal = BuckTransactionsResult.Sum(x => x.Amount)
                
            select 
                    new BucketTransactionsDTO(buck.Id, buck.Name,MonthBucketTotal, BuckTransactionsResult.ToArray())
            ).ToList();

        
        // Make use of the query but change the return type so it matches the required output for the front-end.
        return new SummaryDTO
        {
            Month = month,
            Year = year,
            Buckets = query,
            TotalExpenses = query.Where(x => x.BucketName != Buckets.Salary).Sum(x => x.BucketExpenseTotal),
            TotalIncome = query.Where(x => x.BucketName == Buckets.Salary).Sum(x => x.BucketExpenseTotal),
        };

    }

        }
