using expensetrackerapi.Models;
using Microsoft.EntityFrameworkCore;

namespace expensetrackerapi.helpers;

public class BucketQueries
{
    private readonly ExpenseTrackerContext _db;

    public BucketQueries(ExpenseTrackerContext db)
    {
        _db = db;
    }
    
    

    public async Task<Dictionary<int, int>> SetTotals(string userId)
    {
        return await _db.Transactions
            .Where(t => t.ApplicationUserId == userId)
            .GroupBy(t => t.BucketId)
            .Select(g => new { g.Key, Total = 0 })
            .ToDictionaryAsync(x => x.Key, x => x.Total);
    }

    public async Task<Dictionary<int, int>> GetTotals(string userId)
    {
        return await _db.Transactions
            .Where(t => t.ApplicationUserId == userId)
            .GroupBy(t => t.BucketId)
            .Select(g => new { g.Key, Total = g.Sum(t => t.Amount) })
            .ToDictionaryAsync(x => x.Key, x => x.Total);
    }
}