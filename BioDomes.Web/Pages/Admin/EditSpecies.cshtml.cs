using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using BioDomes.Web.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Admin;

[ValidateAntiForgeryToken]
public class EditSpeciesModel : PageModel
{
    private readonly ISpeciesService _speciesService;
    private readonly ISpeciesClassificationRepository _repositoryClassification;
    private readonly IDietTypeRepository _dietTypeRepository;
    private readonly ILogger<EditSpeciesModel> _logger;
    private readonly IBiodomeService _biodomeService;
    [BindProperty] public InputModel Species { get; set; }

    public IReadOnlyList<string> AllTypes { get; set; } = new List<string>();
    public IReadOnlyList<string> Regime { get; set; } = new List<string>();

    [BindProperty(SupportsGet = true)] public string Slug { get; set; }

    public EditSpeciesModel(
        ISpeciesService speciesService,
        ISpeciesClassificationRepository repositoryClassification,
        IDietTypeRepository dietTypeRepository,
        ILogger<EditSpeciesModel> logger,
        IBiodomeService biodomesService)
    {
        _speciesService = speciesService;
        _repositoryClassification = repositoryClassification;
        _dietTypeRepository = dietTypeRepository;
        _logger = logger;
        _biodomeService = biodomesService;
    }


    public IActionResult OnGet(string slug, string ownerName)
    {
        LoadSelect();
        slug = SanitizeInput(slug);
        ownerName = SanitizeInput(ownerName);
        if (string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(ownerName))
        {
            return NotFound();
        }
        var existing = _speciesService.GetByKebabCase(slug, ownerName);
        if (existing == null)
        {
            _logger.LogInformation("{} not found", slug);
            return NotFound();
        }
        Species = new InputModel
        {
            Name = existing.Nom,
            Type = existing.SpeciesClassification.ToString(),
            Regime = existing.DietType.ToString(),
            Taille = existing.Taille,
            Poids = existing.Poids,
            CurrentAvatarPath = existing.Avatar,
            OwnerName = existing.OwnerName,
        };

        return Page();
    }

    private void LoadSelect()
    {
        AllTypes = _repositoryClassification
            .GetAllSpeciesClassifications()
            .Select(s => s.ToString())
            .ToList();

        Regime = _dietTypeRepository
            .GetAllDietTypes()
            .Select(s => s.ToString())
            .ToList();
    }

    public IActionResult OnPost(string confirm)
    {
        LoadSelect();
        confirm = SanitizeInput(confirm);


        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existing = GetExistingSpecies();

        if (existing is null)
        {
            return NotFound();
        }

        if (!TryParseEnums(out var diet, out var type))
        {
            return Page();
        }

        if (RequiresConfirmation(existing, diet, confirm))
        {
            return Page();
        }

        if (!UpdateSpecies(existing, diet, type))
        {
            ModelState.AddModelError(string.Empty, "Erreur lors de la modification");
            return Page();
        }

        TempData["SuccessMessage"] = "Espèce modifiée avec succès";

        return RedirectToPage("Species");
    }

    private Domains.Core.Species? GetExistingSpecies()
    {
        return _speciesService.GetByKebabCase(Slug, Species.OwnerName);
    }

    private bool TryParseEnums(out DietType diet, out SpeciesClassification type)
    {
        var isDietValid = Enum.TryParse(Species.Regime, out diet);

        if (!isDietValid)
        {
            ModelState.AddModelError("Species.Regime", "Régime invalide");
        }

        var isTypeValid = Enum.TryParse(Species.Type, out type);

        if (!isTypeValid)
        {
            ModelState.AddModelError("Species.Type", "Type invalide");
        }

        return isDietValid && isTypeValid;
    }

    private bool RequiresConfirmation(Domains.Core.Species existing, DietType diet, string confirm)
    {
        var poidsChanged = Math.Abs(existing.Poids - Species.Poids) > 0.001;
        var regimeChanged = existing.DietType != diet;
        var isConfirm = confirm.Equals("true");
        if ((!poidsChanged && !regimeChanged) || isConfirm)
        {
            return false;
        }

        var impacted = _biodomeService.HasSpecies(Slug, Species.OwnerName).Count;
        ViewData["WarningMessage"] = $"Attention : cette modification impactera {impacted} biodôme{(impacted > 1 ? "s" : "")}. Confirmez-vous ?";

        return true;
    }

    private bool UpdateSpecies(Domains.Core.Species existing, DietType diet, SpeciesClassification type)
    {
        var updated = new Domains.Core.Species(
            existing.Nom,
            Species.Taille,
            Species.Poids,
            diet,
            type,
            Species.Avatar?.FileName ?? existing.Avatar,
            existing.IsVisible,
            existing.OwnerName);

        var stream = Species.Avatar?.OpenReadStream();

        return _speciesService.Update(existing, updated, stream);
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$",
            ErrorMessage = "Le nom ne peut contenir que des lettres, espaces ou tirets")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Le type est obligatoire")]
        [RegularExpression("@”^[^<>&]*$", ErrorMessage = "Les tags HTML ne sont pas autorisé")]
        [IsSpeciesClassification]
        public required string Type { get; set; }

        [Required(ErrorMessage = "Le régime est obligatoire")]
        [RegularExpression("@”^[^<>&]*$", ErrorMessage = "Les tags HTML ne sont pas autorisé")]
        [IsSpeciesRegim]
        public required string Regime { get; set; }

        [Required(ErrorMessage = "La taille est obligatoire")]
        [Range(0, 10000, ErrorMessage = "La taille doit être entre 0 et 10000")]
        public required double Taille { get; set; }

        [Required(ErrorMessage = "Le poids est obligatoire")]
        public required double Poids { get; set; }

        [IsImageValid] public IFormFile? Avatar { get; set; }

        public bool Confirmed { get; set; }
        public required string OwnerName { get; set; }
        public string? CurrentAvatarPath { get; set; }
    }
    private string SanitizeInput(string input)
    {
        return Regex.Replace(input, "<.*?>", String.Empty);
    }
}