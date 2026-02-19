namespace expensetrackerapi;
using System.ComponentModel.DataAnnotations;
public record Bucket
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "The Name is required.")]
    [StringLength(50,ErrorMessage = "The Name must be less than 50 characters.")]
    public Buckets Name { get; set; }
    
    [Required(ErrorMessage = "The Icon is required")]
    public required string Icon { get; set; }
    
    [Range(1, 1_000_000, ErrorMessage = "The amount must be between 1 and 1_000_000.")]
    public int Total { get; set; }
}

