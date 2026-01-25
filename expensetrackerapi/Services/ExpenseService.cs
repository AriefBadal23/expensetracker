using expensetrackerapi.Models;

public interface IExpenseService
{
    public object GetTransactions(int? month, int? year, int? bucket, int pageNumber = 1, int pageSize = 3);
    public void CreateTransaction();

    public void DeleteTransaction();


}


namespace expensetrackerapi.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ExpenseTrackerContext _db;

        public ExpenseService(ExpenseTrackerContext db)
        {
            _db = db;
        }
        public object GetTransactions(int? month, int? year, int? bucket, int pageNumber = 1, int pageSize = 3)
        {
            // bucket query string = bucket ID
            var totalRecords = _db.Transactions.Count();
            if (month.HasValue && year.HasValue && !bucket.HasValue)
            {
                var month_transactions = _db.Transactions
                .Where(_ => _.Created_at.Month == month && _.Created_at.Year == year)

                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(_ => _.Created_at).ToList();

                // TODO: ResponseDTO
                return new
                {
                    Total = month_transactions.Count,
                    Transactions = month_transactions
                };
            }
            else if (!month.HasValue && bucket.HasValue)
            {
                var bucket_transactions = _db.Transactions
                .Where(_ => _.BucketId == bucket)

                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(_ => _.BucketId).ToList();

                // TODO: ResponseDTO
                return new
                {
                    Total = totalRecords,
                    Transactions = bucket_transactions
                };
            }


            else if (month.HasValue && year.HasValue && bucket.HasValue)
            {
                var month_transactions = _db.Transactions
                .Where(_ => _.Created_at.Month == month && _.Created_at.Year == year && _.BucketId == bucket)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(_ => _.Created_at).ToList();
                return new
                {
                    Total = month_transactions.Count,
                    Transactions = month_transactions
                };
            }
            else if (month.HasValue && year.HasValue)
            {
                var month_transactions = _db.Transactions
                .Where(_ => _.Created_at.Month == month && _.Created_at.Year == year)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(_ => _.Created_at).ToList();

                // TODO: ResponseDTO
                return new
                {
                    Total = month_transactions.Count,
                    Transactions = month_transactions
                };
            }


            // No month is provided.
            var transactions = _db.Transactions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(_ => _.Created_at).ToList();
            return new
            {
                transactions,
                Total = totalRecords,

            };

        }

        public void CreateTransaction()
        {

        }

        public void DeleteTransaction()
        {

        }


    }
}