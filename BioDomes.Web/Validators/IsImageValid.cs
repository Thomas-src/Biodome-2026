using System.ComponentModel.DataAnnotations;

namespace BioDomes.Web.Validators;

public class IsImageValid : ValidationAttribute
{
    private static readonly long _maxSizeInBytes = 2 * 1024 * 1024;
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is not IFormFile file)
        {
            return new ValidationResult("Fichier invalide");
        }

        var allowed = new[]
        {
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/webp"
        };

        if (!allowed.Contains(file.ContentType))
        {
            return new ValidationResult(
                "Seuls les formats JPG, PNG, GIF et WebP sont acceptés.");
        }

        if (file.Length > _maxSizeInBytes)
        {
            return new ValidationResult(
                $"L'image ne peut pas dépasser {_maxSizeInBytes / 1024 / 1024} Mo.");
        }

        return ValidationResult.Success;
    }
}