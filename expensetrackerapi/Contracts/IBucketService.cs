using expensetrackerapi.DTO;
using expensetrackerapi.Results;

namespace expensetrackerapi.Contracts;

public interface IBucketService
{
    public Task<Result<BucketResponseDto>> GetSummary(int month, int year);
    public Task<Result<List<Bucket>>> GetBuckets();
}