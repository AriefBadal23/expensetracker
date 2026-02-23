using System.Text.Json.Serialization;
using NodaTime;
namespace expensetrackerapi.DTO;

public class CreateTransactionDto
{
    [JsonPropertyName("bucketId")]
    public int BucketId { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
    [JsonPropertyName("created_at")]
    public LocalDate CreatedAt { get; set; }
    
    [JsonPropertyName("isIncome")]
    public bool IsIncome { get; set; }
}
