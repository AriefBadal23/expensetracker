using expensetrackerapi.Models;

namespace expensetrackerapi;
using System.ComponentModel.DataAnnotations;
public record Bucket
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "The Name is required.")]
    public Buckets Name { get; set; }
    
    [Required(ErrorMessage = "The Icon is required")]
    public required string Icon { get; set; }
    
    [Range(0, 1_000_000, ErrorMessage = "The amount must be between 1 and 1_000_000.")]
    public int Total { get; set; }
    
    [Required(ErrorMessage = "Type is required.")]
    public BucketTypes Type { get; set; }
}

