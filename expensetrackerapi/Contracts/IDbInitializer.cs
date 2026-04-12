using expensetrackerapi.Models;

namespace expensetrackerapi.Contracts;

public interface IDbInitializer
{
    public Task SeedAsync(ExpenseTrackerContext context);
}