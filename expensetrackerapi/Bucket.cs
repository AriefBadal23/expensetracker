namespace expensetrackerapi.Controllers;

public record Bucket
{
    public int Id { get; set; }
    public int Total { get; set; }
    public required string icon { get; set; }
    public required Buckets Name { get; set; }
}

