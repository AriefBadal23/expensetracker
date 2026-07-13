namespace expensetrackerapi.Models;


public record BucketTransaction(int BucketId, string BucketName, BucketTypes BucketType, decimal BucketExpenseTotal, Transaction[] Transactions);
