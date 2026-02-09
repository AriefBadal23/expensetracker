using expensetrackerapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace expensetrackerapi.Services;

public interface IBucketService
{
    public SummaryDTO GetSummary(Buckets bucket, int month, int year);
    public List<Bucket> GetBuckets(Buckets bucket);
}


public record BucketTransactionsDTO(int BucketId,Buckets BucketName, Transaction[] Transactions);

public record SummaryDTO
{
    public int Month { get; set; }
    public int Year { get; set; }
    public List<BucketTransactionsDTO> Buckets { get; set; }
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

    public SummaryDTO GetSummary(Buckets bucket, int month, int year)
    {
        if (month != 0 && year != 0)
        {
            var query = (from buck in _db.Buckets
                join transaction in _db.Transactions on buck.Id equals transaction.BucketId into bucketTransactions
                     
                let BuckTransactions = bucketTransactions
                    .Where(t => t.Created_at.Month == month && t.Created_at.Year == year)
                     
                select 
                    new BucketTransactionsDTO(buck.Id, buck.Name, BuckTransactions.ToArray())
                ).ToList();
            
            // Make use of the query but change the return type so it matches the required output for the front-end.
            return new SummaryDTO
            {
                Month = month,
                Year = year,
                Buckets = query
            };
        }

        return new SummaryDTO();
    }

        }
