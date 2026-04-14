using expensetrackerapi;
using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using expensetrackerapi.Results;

namespace expensetrackerapi.Contracts;

public interface IExpenseService
{
    public Task<Result<object>> GetTransactions(string userId, int? month, int? year, int? bucket, int pageNumber = 1, int pageSize = 3);
    public Task<Result<ResponseTransactionDTo>> CreateTransaction(string userId, RequestTransactionDto transaction);

    public Task<Result<bool>> DeleteTransaction(string userId, int transactionID);

    public Task<Result<ResponseTransactionDTo?>> GetTransactionByID(int Id); // Task is enough; no async modifier required.

    public Task<Result<ResponseTransactionDTo?>> UpdateTransaction(string userId, int id, UpdateTransactionDto transaction);
}