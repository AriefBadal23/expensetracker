namespace expensetrackerapi.Models;


public record BucketTransaction(int BucketId, string BucketName, BucketTypes BucketType, int BucketExpenseTotal, Transaction[] Transactions);
