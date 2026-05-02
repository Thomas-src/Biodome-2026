using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Core;

namespace BioDomes.Web.Validators;

public class IsSpeciesRegimAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {

        if (value == null)
        {
            return new ValidationResult("Le type de régime est obligatoire");
        }

        var regim = value.ToString();

        var isValid = Enum.TryParse(typeof(DietType), regim, true, out _);

        if (!isValid)
        {
            return new ValidationResult("Régime invalide");
        }

        return ValidationResult.Success;

    }
}