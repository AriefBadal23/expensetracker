using expensetrackerapi.Models;

namespace expensetrackerapi.DTO;

public record BucketResponseDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public List<BucketTransaction> Buckets { get; set; }

    public int TotalIncome { get; set; }
    public int TotalExpenses { get; set; }
}