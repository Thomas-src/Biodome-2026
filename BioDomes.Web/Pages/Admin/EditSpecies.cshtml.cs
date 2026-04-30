using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Web.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Admin;

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

        var existing = _speciesService.GetByKebabCase(slug,ownerName);
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
        _logger.LogInformation("POST slug = {slug}", Slug);
        LoadSelect();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existing = _speciesService.GetByKebabCase(Slug,Species.OwnerName);
        if (existing == null)
        {
            return NotFound();
        }
            

        if (!Enum.TryParse<DietType>(Species.Regime, out var diet))
        {
            ModelState.AddModelError("Species.Regime", "Régime invalide");
            return Page();
        }

        if (!Enum.TryParse<SpeciesClassification>(Species.Type, out var type))
        {
            ModelState.AddModelError("Species.Type", "Type invalide");
            return Page();
        }

        var poidsChanged = Math.Abs(existing.Poids - Species.Poids) > 0.001;
        var regimeChanged = existing.DietType != diet;
        var isConfirm = confirm.Equals("true");

        if ((poidsChanged || regimeChanged) && !isConfirm)
        {
            var impacted = _biodomeService.HasSpecies(Slug, Species.OwnerName).Count;
            ViewData["WarningMessage"] = $"Attention : cette modification impactera {impacted} biodôme{(impacted > 1 ? "s" : "")}. Confirmez-vous ?"; 

            return Page();
        }

        var updated = new Domains.Core.Species(
            existing.Nom,
            Species.Taille,
            Species.Poids,
            diet,
            type,
            Species.Avatar?.FileName ?? existing.Avatar,
            existing.IsVisible,
            existing.OwnerName
        );

        var stream = Species.Avatar?.OpenReadStream();

        if (_speciesService.Update(existing, updated, stream))
        {
            TempData["SuccessMessage"] = "Espèce modifiée avec succès";
            return RedirectToPage("Species");
        }

        ModelState.AddModelError("", "Erreur lors de la modification");
        return Page();
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$", ErrorMessage = "Le nom ne peut contenir que des lettres, espaces ou tirets")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Le type est obligatoire")]
        [IsSpeciesClassification]
        public string Type { get; set; }

        [Required(ErrorMessage = "Le régime est obligatoire")]
        public string Regime { get; set; }

        [Required(ErrorMessage = "La taille est obligatoire")]
        [Range(0, 10000, ErrorMessage = "La taille doit être entre 0 et 10000")]
        public double Taille { get; set; }

        [Required(ErrorMessage = "Le poids est obligatoire")]
        public double Poids { get; set; }

        public IFormFile? Avatar { get; set; }
        public bool Confirmed { get; set; } = false;
        public string OwnerName { get; set; }
        public string? CurrentAvatarPath { get; set; }
    }
    
    
}