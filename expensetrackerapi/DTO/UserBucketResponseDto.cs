using expensetrackerapi.Models;

namespace expensetrackerapi.DTO;

public class UserBucketResponseDto
{
    public int BucketTotal { get; set; }
    public required Bucket Bucket { get; set; }


}