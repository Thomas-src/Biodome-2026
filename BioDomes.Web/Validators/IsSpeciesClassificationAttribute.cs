using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Core;

namespace BioDomes.Web.Validators;

public class IsSpeciesClassificationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {

        if (value == null)
        {
            return new ValidationResult("La classification est obligatoire");
        }

        string classification = value.ToString();

        bool isValid = Enum.TryParse(typeof(SpeciesClassification), classification, true, out _);

        if (!isValid)
        {
            return new ValidationResult("Classification invalide");
        }

        return ValidationResult.Success;

    }
}