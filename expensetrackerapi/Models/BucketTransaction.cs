namespace expensetrackerapi.Models;


public record BucketTransaction(int BucketId, Buckets BucketName,BucketTypes BucketType, int BucketExpenseTotal, Transaction[] Transactions);
