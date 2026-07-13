using expensetrackerapi.Models;

namespace expensetrackerapi.DTO;

public class UserBucketResponseDto
{
    public decimal BucketTotal { get; set; }
    public required BucketResponseDto Bucket { get; set; }


}