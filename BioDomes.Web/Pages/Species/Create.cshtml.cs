using System.ComponentModel.DataAnnotations;
using BioDomes.Domains;
using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BioDomes.Web.Validators;

namespace BioDomes.Web.Pages.Species;

public class Create : PageModel
{
    private readonly ISpeciesRepository _repository;

    [BindProperty]
    public InputModel Species { get; set; }

    [BindProperty]
    public IList<string> AllTypes { get; set; } = new List<string>();

    public Create(ISpeciesRepository repository)
    {
        _repository = repository;
    }

    public void OnGet()
    {
        _repository.GetAllSpeiesTypes();
    }
/*
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

      SqlRepository sqlRepository = new SqlRepository();
        IList<Espece> especes = sqlRepository.GetallEspece();

        Espece created = new Espece(
            Species.Type,
            Species.Name,
            Species.Regime,
            Species.Taille.Value
        );

        bool exists = especes.Any(e => e.Equals(created));

        if (exists)
        {
            ModelState.AddModelError("Species.Name", "Cette espèce existe déjà.");
            return Page();
        }

        sqlRepository.Save(created);

        TempData["SuccessMessage"] = $"L'espèce {Species.Name} a été ajoutée avec succès !";
        return RedirectToPage("Species");

    }
}
*/
    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$",
            ErrorMessage = "Le nom ne peut contenir que des lettres, espaces ou tirets")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Le type est obligatoire")]
        [IsSpeciesClassification]
        [RegularExpression(@"^[a-zA-Z\s\-]+$",
            ErrorMessage = "Le type ne peut contenir que des lettres, espaces ou tirets")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Le régime est obligatoire")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$",
            ErrorMessage = "Le régime ne peut contenir que des lettres, espaces ou tirets")]
        public string Regime { get; set; }

        [Required(ErrorMessage = "La taille est obligatoire")]
        [Range(0, 10000, ErrorMessage = "La taille doit être entre 0 et 10000")]
        public int? Taille { get; set; }
    }
}
