using expensetrackerapi;
using expensetrackerapi.DTO;
using expensetrackerapi.Models;

public interface IExpenseService
{
    public object GetTransactions(int? month, int? year, int? bucket, int pageNumber = 1, int pageSize = 3);
    public ResponseTransactionDTo? CreateTransaction(RequestTransactionDto transaction);

    public bool DeleteTransaction(int transactionID);

    public ResponseTransactionDTo? GetTransactionByID(int Id);

    public ResponseTransactionDTo? UpdateTransaction(Transaction transaction);
}