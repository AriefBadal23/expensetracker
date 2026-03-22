using expensetrackerapi;
using expensetrackerapi.DTO;
using expensetrackerapi.Models;

public interface IExpenseService
{
    public Task<object> GetTransactions(int? month, int? year, int? bucket, int pageNumber = 1, int pageSize = 3);
    public Task<ResponseTransactionDTo?> CreateTransaction(RequestTransactionDto transaction);

    public Task<bool> DeleteTransaction(int transactionID);

    public Task<ResponseTransactionDTo?> GetTransactionByID(int Id); // Task is enough; no async modifier required.

    public Task<ResponseTransactionDTo?> UpdateTransaction(Transaction transaction);
}