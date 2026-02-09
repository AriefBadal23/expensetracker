using expensetrackerapi.Models;

namespace expensetrackerapi.Services;

public interface IBucketService
{
    public List<BucketDto> Get(Buckets? bucket, int? month, int? year);
}


public record BucketDto(int? Id, Buckets? Name, string? Icon, int? Month,
                        int? Year, int Total, List<int>? BucketId);



public class BucketService: IBucketService
{
    private readonly ExpenseTrackerContext _db;
    
    public BucketService(ExpenseTrackerContext context)
    {
        _db = context;
    }
    
    public List<BucketDto> Get(Buckets? bucket, int? month, int? year)
    {
        if (!bucket.HasValue)
        {
            var allBuckets = _db.Buckets.Select(x => new BucketDto(x.Id, x.Name,x.Icon,0,0,x.Total,null)).ToList();
            return allBuckets;
        }

        var query =
            from b in _db.Buckets
            join t in _db.Transactions on b.Id equals t.BucketId
            where b.Name == bucket
            select new {Transactions=t, Bucket=b};
        
        if (month.HasValue)
        {
            query = query.Where(x => x.Transactions.Created_at.Month == month);
        }

        if (year.HasValue)
        {
            query = query.Where(x => x.Transactions.Created_at.Year == year);

        }

        var grouped = (from t in query
                group t by new {t.Bucket.Id,t.Bucket.Name, t.Bucket.Icon, t.Transactions.Created_at.Month, t.Transactions.Created_at.Year } // composite keys
                into grouping
                select new BucketDto
                    (
                    grouping.Key.Id,
                    grouping.Key.Name,
                    grouping.Key.Icon,
                    grouping.Key.Month,
                    grouping.Key.Year,
                    grouping.Sum(x => x.Transactions.Amount),
                    grouping.Select(x => x.Transactions.BucketId).ToList()
                    

                ))
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();

        // var labels = grouped.Select(x => year.HasValue ? CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.month) : $"{x.year}-{x.month:00}").ToArray();
        // var totals = grouped.Select(x => x.total).ToArray();
        // var bucketName = grouped.Select(x => x.bucketId).First();

        // var result = new List<object>()
        // {
        //     new
        //     {
        //         Labels = labels,
        //         Bucket = bucketName.First(),
        //         Totals = totals,
        //         Month = month,
        //         Year = year
        //     }
        // };
        return grouped;
    }
    }
