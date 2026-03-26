using expensetrackerapi;
using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using expensetrackerapi.Results;

public interface IExpenseService
{
    public Task<Result<object>> GetTransactions(int? month, int? year, int? bucket, int pageNumber = 1, int pageSize = 3);
    public Task<Result<ResponseTransactionDTo?>> CreateTransaction(RequestTransactionDto transaction);

    public Task<Result<bool>> DeleteTransaction(int transactionID);

    public Task<Result<ResponseTransactionDTo?>> GetTransactionByID(int Id); // Task is enough; no async modifier required.

    public Task<Result<ResponseTransactionDTo?>> UpdateTransaction(Transaction transaction);
}