using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using expensetrackerapi.Results;

namespace expensetrackerapi.Contracts;

public interface IBucketService
{
    public Task<Result<BucketResponseDto>> GetSummary(int month, int year);
    public Task<Result<List<Bucket>>> GetBuckets();
    public Task<Result<List<UserBucketResponseDto>>> GetBucketsByUserId(string? userId);
}