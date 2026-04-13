using System.ComponentModel.DataAnnotations;

namespace expensetrackerapi.Models;

public record Bucket
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Name is required.")]
    public Buckets Name { get; set; }

    [Required(ErrorMessage = "The Icon is required")]
    public required string Icon { get; set; }

    [Required(ErrorMessage = "Type is required.")]
    public BucketTypes Type { get; set; }
    
    public List<ApplicationUser> Users { get; } = [];
    public List<UserBuckets> UserBuckets { get; } = [];
}

