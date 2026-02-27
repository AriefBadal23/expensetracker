using NodaTime;
namespace expensetrackerapi.DTO;

public class ResponseTransactionDTo
{
    public int Id { get; set; }
    public int BucketId { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; } = null!;
    public int Amount { get; set; }
    public LocalDate Created_at { get; set; }
}

