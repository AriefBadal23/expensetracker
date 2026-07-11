using expensetrackerapi.DTO;
using expensetrackerapi.Models;
using expensetrackerapi.Results;

namespace expensetrackerapi.Contracts;

public interface IBucketService
{
    public Task<Result<BucketSummaryResponseDto>> GetSummary(string userId, int month, int year);
    public Task<Result<List<UserBucketResponseDto>>> GetBucketsByUserId(string? userId);
    public Task<Result<UserBucketResponseDto>> CreateBucket(string? userId, BucketRequestDto bucket);
    public Task<Result<bool>> DeleteBucket(string? userId, int bucketid);

    public Task<Result<BucketResponseDto>> UpdateBucket(int bucketId, string? userId, BucketRequestDto bucket);
}