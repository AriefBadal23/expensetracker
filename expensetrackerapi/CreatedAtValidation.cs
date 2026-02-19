using System.ComponentModel.DataAnnotations;
using expensetrackerapi.Models;
using NodaTime;


namespace expensetrackerapi;

public class CreatedAtValidation : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is LocalDate date)
        {
            if (date.Year > DateTime.Now.Year)
            {
                return new ValidationResult("The created date year must not be later than this year.");
            }
        }
        return ValidationResult.Success;
    }
}

