using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Admin;

public class EditEquipementModel : PageModel
{
    private readonly IEquipementService _equipementService;
    private readonly IBiodomeService _biodomeService;
    private readonly ILogger<EditEquipementModel> _logger;

    [BindProperty] public InputModel Equipement { get; set; }

    public IReadOnlyList<string> AllConsumptions { get; set; } = new List<string>();
    public IReadOnlyList<string> AllProductions { get; set; } = new List<string>();

    [BindProperty(SupportsGet = true)] public string Slug { get; set; }

    public EditEquipementModel(
        IEquipementService equipementService,
        IBiodomeService biodomeService,
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

        _logger.LogInformation("{} confirmed", confirm);
        if (!ModelState.IsValid)
        {
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    _logger.LogWarning(
                        "ModelState error on {Field}: {Error}",
                        state.Key,
                        error.ErrorMessage
                    );
                }
            }
            return Page();
        }


        var existing = _equipementService.GetByKebabCase(Slug, Equipement.OwnerName);
        if (existing == null)
        {
            _logger.LogInformation("{} not found for user {}", Slug, Equipement.OwnerName);
            return NotFound();
        }


        if (!Enum.TryParse<Consumptions>(Equipement.Consumption, out var consumption))
        {
            ModelState.AddModelError("Equipement.Consumption", "Consommation invalide");
            return Page();
        }

        if (!Enum.TryParse<Production>(Equipement.Production, out var production))
        {
            ModelState.AddModelError("Equipement.Production", "Production invalide");
            return Page();
        }

        var consumptionChanged = existing.Consumption != consumption;
        var productionChanged = existing.Production != production;
        var isConfirm = confirm.Equals("true");

        if ((consumptionChanged || productionChanged) && !isConfirm)
        {
            var impacted = _biodomeService.HasEquipement(Slug, existing.OwnerName).Count;
            ViewData["WarningMessage"] =
                $"Attention : cette modification impactera {impacted} biodôme{(impacted > 1 ? "s" : "")}. Confirmez-vous ?";
            return Page();
        }

        var updated = new Equipement(
            existing.Name,
            consumption,
            production,
            Equipement.Avatar?.FileName ?? existing.Avatar,
            existing.Visibility,
            existing.OwnerName
        );

        var stream = Equipement.Avatar?.OpenReadStream();

        _equipementService.Update(existing, updated, stream);

        TempData["SuccessMessage"] = "Équipement modifié avec succès";
        return RedirectToPage("Equipements");
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La consommation est obligatoire")]
        public string Consumption { get; set; }

        [Required(ErrorMessage = "La production est obligatoire")]
        public string Production { get; set; }

        public IFormFile? Avatar { get; set; }
        public bool Confirmed { get; set; } = false;
        public string? CurrentAvatarPath { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string OwnerName { get; set; }
    }
}