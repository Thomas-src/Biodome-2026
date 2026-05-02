using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using BioDomes.Web.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Admin;

[ValidateAntiForgeryToken]
public class EditEquipementModel : PageModel
{
    private readonly IEquipementService _equipementService;
    private readonly IBiodomeService _biodomeService;
    private readonly ILogger<EditEquipementModel> _logger;

    [BindProperty] public InputModel Equipement { get; set; }

    public IReadOnlyList<string> AllConsumptions { get; set; } = new List<string>();
    public IReadOnlyList<string> AllProductions { get; set; } = new List<string>();

    [BindProperty(SupportsGet = true)] public string Slug { get; set; }

    public EditEquipementModel(IEquipementService equipementService, IBiodomeService biodomeService,
        ILogger<EditEquipementModel> logger)
    {
        _equipementService = equipementService;
        _biodomeService = biodomeService;
        _logger = logger;
    }

    public IActionResult OnGet(string slug, string ownerName)
    {
        LoadSelects();

        var existing = _equipementService.GetByKebabCase(slug, ownerName);
        if (existing == null)
        {
            _logger.LogInformation("{} not found", slug);
            return NotFound();
        }

        Equipement = new InputModel
        {
            Name = existing.Name,
            Consumption = existing.Consumption.ToString(),
            Production = existing.Production.ToString(),
            CurrentAvatarPath = existing.Avatar,
            OwnerName = existing.OwnerName,
        };

        return Page();
    }

    private void LoadSelects()
    {
        AllConsumptions = Enum.GetNames<Consumptions>().ToList();
        AllProductions = Enum.GetNames<Production>().ToList();
    }

    public IActionResult OnPost(string confirm)
    {
        LoadSelects();

        if (!ModelState.IsValid)
        {
            LogModelStateErrors();
            return Page();
        }

        var existing = GetExistingEquipement();

        if (existing is null)
        {
            return NotFound();
        }

        if (!TryParseEnums(out var consumption, out var production))
        {
            return Page();
        }

        if (RequiresConfirmation(existing, consumption, production, confirm))
        {
            return Page();
        }

        UpdateEquipement(existing, consumption, production);

        TempData["SuccessMessage"] = "Équipement modifié avec succès";

        return RedirectToPage("Equipements");
    }

    private void LogModelStateErrors()
    {
        foreach (var state in ModelState)
        {
            foreach (var error in state.Value.Errors)
            {
                _logger.LogWarning("ModelState error on {Field}: {Error}", state.Key, error.ErrorMessage);
            }
        }
    }

    private Equipement? GetExistingEquipement()
    {
        var existing = _equipementService.GetByKebabCase(Slug, Equipement.OwnerName);

        if (existing is null)
        {
            _logger.LogInformation("{Slug} not found for user {Owner}", Slug, Equipement.OwnerName);
        }

        return existing;
    }

    private bool TryParseEnums(out Consumptions consumption, out Production production)
    {
        var isConsumptionValid = Enum.TryParse(Equipement.Consumption, out consumption);

        if (!isConsumptionValid)
        {
            ModelState.AddModelError("Equipement.Consumption", "Consommation invalide");
        }

        var isProductionValid = Enum.TryParse(Equipement.Production, out production);

        if (!isProductionValid)
        {
            ModelState.AddModelError("Equipement.Production", "Production invalide");
        }

        return isConsumptionValid && isProductionValid;
    }

    private bool RequiresConfirmation(Equipement existing, Consumptions consumption, Production production, string confirm)
    {
        var consumptionChanged = existing.Consumption != consumption;

        var productionChanged = existing.Production != production;

        var isConfirm = confirm.Equals("true");

        if ((!consumptionChanged && !productionChanged) || isConfirm)
        {
            return false;
        }

        var impacted = _biodomeService.HasEquipement(Slug, existing.OwnerName).Count;
        ViewData["WarningMessage"] = $"Attention : cette modification impactera {impacted} biodôme{(impacted > 1 ? "s" : "")}. Confirmez-vous ?";

        return true;
    }

    private void UpdateEquipement(Equipement existing, Consumptions consumption, Production production)
    {
        var updated = new Equipement(
            existing.Name,
            consumption,
            production,
            Equipement.Avatar?.FileName ?? existing.Avatar,
            existing.Visibility,
            existing.OwnerName);
        var stream = Equipement.Avatar?.OpenReadStream();
        _equipementService.Update(existing, updated, stream);
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "La consommation est obligatoire")]
        public required string Consumption { get; set; }

        [Required(ErrorMessage = "La production est obligatoire")]
        public required string Production { get; set; }

        [IsImageValid] public IFormFile? Avatar { get; set; }
        public bool Confirmed { get; set; }
        public string? CurrentAvatarPath { get; set; }

        [BindProperty(SupportsGet = true)] public required string OwnerName { get; set; }
    }
}