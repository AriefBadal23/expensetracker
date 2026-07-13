using NodaTime;
namespace expensetrackerapi.DTO;

public class ResponseTransactionDTo
{
    public int Id { get; set; }
    public int BucketId { get; set; }
    public string Description { get; set; } = null!;
    public decimal Amount { get; set; }
    public LocalDate CreatedAt { get; set; }
}

