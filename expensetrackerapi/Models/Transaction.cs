using expensetrackerapi.Validation;
using NodaTime;
namespace expensetrackerapi.Models;
using System.ComponentModel.DataAnnotations;

public record Transaction
{
    public int Id { get; set; }
    public int BucketId { get; set; }
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "The Description is required.")]
    [StringLength(50,ErrorMessage = "The description must be less than 50 characters.")]
    public string? Description { get; set; }
    
    [Range(0, 100_000, ErrorMessage = "The amount must be between 0 and 100_000.")]
    public int Amount { get; set; }
    
    [CreatedAtValidation]
    [Required(ErrorMessage = "Created_at is required.")]
    public LocalDate Created_at { get; set; }
    
}