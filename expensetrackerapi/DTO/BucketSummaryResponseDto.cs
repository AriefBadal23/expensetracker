using expensetrackerapi.Models;

namespace expensetrackerapi.DTO;

public record BucketSummaryResponseDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public List<BucketTransaction>? Buckets { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
}