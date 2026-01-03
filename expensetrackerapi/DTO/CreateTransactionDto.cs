using System.Text.Json.Serialization;
using NodaTime;

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
    
    [JsonPropertyName("isExpense")]
    public bool IsExpense { get; set; }
}
