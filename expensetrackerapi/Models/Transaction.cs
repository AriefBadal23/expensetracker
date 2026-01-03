using NodaTime;

namespace expensetrackerapi;

public record Transaction
{
    public int Id { get; set; }
    public int BucketId { get; set; }
    public int UserId { get; set; }
    public required string Description { get; set; }
    public int Amount { get; set; }
    public LocalDate Created_at { get; set; }
    public bool isExpense { get; set; }
}