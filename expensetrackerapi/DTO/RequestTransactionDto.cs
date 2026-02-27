using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using expensetrackerapi.Validation;
using NodaTime;
namespace expensetrackerapi.DTO;

public class RequestTransactionDto
{
    [JsonPropertyName("bucketId")]
    public int BucketId { get; set; }
    
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    
    [JsonPropertyName("description")]
    [Required(ErrorMessage = "The Description is required.")]
    [StringLength(50,ErrorMessage = "The description must be less than 50 characters.")]
    public string Description { get; set; } = null!;
    
    [JsonPropertyName("amount")]
    [Range(0, 100_000, ErrorMessage = "The amount must be between 0 and 100_000.")]
    public int Amount { get; set; }
    
    [CreatedAtValidation]
    [Required(ErrorMessage = "Created_at is required.")]
    public LocalDate Created_at { get; set; }
    
    [JsonPropertyName("isIncome")]
    [Required(ErrorMessage = "isIncome is required.")]
    public bool IsIncome { get; set; }
}
