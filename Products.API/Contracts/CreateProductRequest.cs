using System.ComponentModel.DataAnnotations;

namespace Products.API.Contracts;

public sealed class CreateProductRequest : IValidatableObject
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
    public string Name { get; init; } = string.Empty;

    [Range(typeof(decimal), "0.01", "999999999.99",
        ErrorMessage = "Price must be between 0.01 and 999999999.99.")]
    public decimal Price { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrWhiteSpace(Name) && Name.Trim().Length < 2)
        {
            yield return new ValidationResult(
                "Product name must have at least two non-padding characters.",
                new[] { nameof(Name) });
        }
    }
}
