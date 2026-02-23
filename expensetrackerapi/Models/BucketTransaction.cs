namespace expensetrackerapi.Models;

public record BucketTransaction(int BucketId, Buckets BucketName, int BucketExpenseTotal, Transaction[] Transactions);
