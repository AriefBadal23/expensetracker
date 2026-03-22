using expensetrackerapi.DTO;

namespace expensetrackerapi.Services;

public interface IBucketService
{
    public Task<BucketResponseDto> GetSummary(int month, int year);
    public Task<List<Bucket>> GetBuckets();
}