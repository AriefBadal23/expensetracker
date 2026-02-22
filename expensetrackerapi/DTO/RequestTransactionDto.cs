using System.Text.Json.Serialization;
using NodaTime;
namespace expensetrackerapi.DTO;

public class RequestTransactionDto
{
    [JsonPropertyName("bucketId")]
    public int BucketId { get; set; }
    
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
    [JsonPropertyName("created_at")]
    public LocalDate Created_at { get; set; }
    
    [JsonPropertyName("isIncome")]
    public bool IsIncome { get; set; }
}
