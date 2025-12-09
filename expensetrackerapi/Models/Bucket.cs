namespace expensetrackerapi;

public record Bucket
{
    public int Id { get; set; }
    public required string Icon { get; set; }
    public required Buckets Name { get; set; }
}

