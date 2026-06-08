using System.ComponentModel.DataAnnotations;
using expensetrackerapi.Models;

namespace expensetrackerapi.DTO;

public class BucketRequestDto
{
    [Required(ErrorMessage = "The Name is required.")]
    [StringLength(15, ErrorMessage = "the description must be less than 15 characters.")]
    public required  string Name { get; set; }
    
    [Required(ErrorMessage = "The Icon is required.")]
    [RegularExpression(@"\S+", ErrorMessage = "The Icon must not be empty or whitespace.")]
    public required string Icon { get; set; } = null!;
    
    [Required(ErrorMessage = "The Type is required.")]
    public required BucketTypes Type { get; set; }
}