using expensetrackerapi.DTO;

namespace expensetrackerapi.Services;

public interface IBucketService
{
    public BucketResponseDto GetSummary(int month, int year);
    public List<Bucket> GetBuckets(Buckets bucket);
}