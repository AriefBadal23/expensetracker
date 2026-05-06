using System.ComponentModel.DataAnnotations;
using expensetrackerapi.Validation;
using NodaTime;

namespace expensetrackerapi.DTO;

public class UpdateTransactionDto
{
    public required int BucketId { get; set; }

    [Required(ErrorMessage = "The Description is required.")]
    [StringLength(50, ErrorMessage = "The description must be less than 50 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "The Amount is required.")]
    [Range(0, 100_000, ErrorMessage = "The amount must be between 0 and 100_000.")]
    public required int Amount { get; set; }

    [CreatedAtValidation]
    [Required(ErrorMessage = "Created_at is required.")]
    public required LocalDate CreatedAt { get; set; }
}