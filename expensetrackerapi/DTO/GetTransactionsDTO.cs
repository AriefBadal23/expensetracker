using System.Text.Json.Serialization;
namespace expensetrackerapi.DTO;

public record GetTransactionsDTO
{
    [JsonPropertyName("month")]
    public int? Month { get; set; }
    [JsonPropertyName("year")]
    public int? Year { get; set; } = null!;
    [JsonPropertyName("bucket")]
    public int? Bucket { get; set; }
    [JsonPropertyName("pageSize")] 
    public int pageSize { get; set; } = 3;
    [JsonPropertyName("pageNumber")]
    public int pageNumber { get; set; } = 1;
}