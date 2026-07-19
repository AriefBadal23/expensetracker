using expensetrackerapi.Models;

namespace expensetrackerapi.DTO;

public class BucketResponseDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Icon { get; set; }
    public required BucketTypes Type { get; set; }
}