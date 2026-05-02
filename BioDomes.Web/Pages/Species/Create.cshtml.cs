using System.Collections;
using System.ComponentModel.DataAnnotations;
using BioDomes.Domains;
using BioDomes.Domains.Constants;
using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using BioDomes.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BioDomes.Web.Validators;
using Microsoft.AspNetCore.Authorization;

namespace BioDomes.Web.Pages.Species;

[Authorize(Roles = AppRoles.Admin + "," + AppRoles.Research)]
public class Create : PageModel
{
    private readonly ISpeciesClassificationRepository _repositoryClassification;
    private readonly IDietTypeRepository _dietTypeRepository;
    private readonly ISpeciesService _speciesService;

    [BindProperty] public InputModel Species { get; set; }

    [BindProperty] public IReadOnlyList<string> AllTypes { get; set; } = new List<string>();

    [BindProperty] public IReadOnlyList<string> Regime { get; set; } = new List<string>();

    public Create(ISpeciesService service, ISpeciesClassificationRepository repositoryClassification,
        IDietTypeRepository dietTypeRepository)
    {
        _speciesService = service;
        _repositoryClassification = repositoryClassification;
        _dietTypeRepository = dietTypeRepository;
    }

    public void OnGet()
    {
        LoadSelect();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            LoadSelect();
            return Page();
        }

        if (!Enum.TryParse<DietType>(Species.Regime, out var diet))
        {
            ModelState.AddModelError("Species.Regime", "Régime invalide");
            LoadSelect();
            return Page();
        }

        if (!Enum.TryParse<SpeciesClassification>(Species.Type, out var type))
        {
            ModelState.AddModelError("Species.Type", "Type invalide");
            LoadSelect();
            return Page();
        }

        // "images/species/lion.jpg"

        var created = new Domains.Core.Species(
            Species.Name,
            Species.Taille,
            Species.Poids,
            diet,
            type,
            Species.Avatar.FileName,
            false,
            User.Identity.Name
        );

        try
        {
            _speciesService.Save(created, Species.Avatar.OpenReadStream());
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", $"{e.Message}");
            LoadSelect();
            return Page();
        }

        TempData["SuccessMessage"] = $"L'espèce {Species.Name} a été ajoutée avec succès !";
        return RedirectToPage("Species");
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


    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$",
            ErrorMessage = "Le nom ne peut contenir que des lettres, espaces ou tirets")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Le type est obligatoire")]
        [IsSpeciesClassification]
        public required string Type { get; set; }

        [Required(ErrorMessage = "Le régime est obligatoire")]
        [IsSpeciesRegim]
        public required string Regime { get; set; }

        [Required(ErrorMessage = "La taille est obligatoire")]
        [Range(1, 10000, ErrorMessage = "La taille doit être entre 1 et 10000")]
        public required double Taille { get; set; }

        [Required(ErrorMessage = "Le poids est obligatoire")]
        [Range(1, double.MaxValue, ErrorMessage = "Le poids doit être au moins de 1g")]
        public required double Poids { get; set; }

        [Required(ErrorMessage = "Un avatar est obligatoire")]
        [IsImageValid]
        public required IFormFile Avatar { get; set; }
    }
}