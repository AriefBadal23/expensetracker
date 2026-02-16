using expensetrackerapi;

public interface IExpenseService
{
    public object GetTransactions(int? month, int? year, int? bucket, int pageNumber = 1, int pageSize = 3);
    public bool CreateTransaction(CreateTransactionDto dto);

    public bool DeleteTransaction(int transactionID);

    public Transaction? GetTransactionByID(int Id);

    public Transaction? UpdateTransaction(Transaction transaction);
}