namespace expensetrackerapi.Models;

public class UserBuckets
{
    // both FKs are automatically picked up and are configured as the composite PK for the join entity type.
    // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#many-to-many-with-class-for-join-entity
    public string ApplicationUserId { get; set; }
    public int BucketId { get; set; }
    public int Total { get; set; } 
    
}