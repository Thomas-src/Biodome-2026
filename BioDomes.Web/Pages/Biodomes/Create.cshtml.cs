using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Core;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Web.Pages.Biodomes;

[Authorize]
public class Create : PageModel
{
    private readonly IBiodomeService _service;

    public Create(IBiodomeService service)
    {
        _service = service;
    }

    [BindProperty]
    public InputModel Biome { get; set; }

    public void OnGet()
    {
        
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        double humiditeMax = _service.CalculateHumiditeMax(Biome.temperature.Value);

        if (Biome.humidite.Value > humiditeMax)
        {
            ModelState.AddModelError("Biome.humidite", $"Un biome avec une température de {Biome.temperature.Value}° peut avoir une humidité max de {humiditeMax} g/m³");
            return Page();
        }

        Biodome created = new Biodome(
            -1,
            Biome.Name,
            Biome.temperature.Value,
            Biome.humidite.Value,
            Biome.Avatar.FileName,
            new List<Domains.Core.Species>(),
            new List<Equipement>(),
            User.Identity.Name
            );
        
        _service.Save(created, Biome.Avatar.OpenReadStream());

        TempData["SuccessMessage"] = $"L'espèce {Biome.Name} a été ajoutée avec succès !";
        return RedirectToPage("Biodomes");
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$",
            ErrorMessage = "Le nom ne peut contenir que des lettres, espaces ou tirets")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La temperature est obligatoire")]
        [Range(-100, 100, ErrorMessage = "La taille doit être entre -100 et 100")]
        public double? temperature { get; set; }

        [Required(ErrorMessage = "L'humidité est obligatoire")]
        [Range(-100, 100, ErrorMessage = "L'humitidé doit être entre -100 et 100")]
        public double? humidite { get; set; }

        [Required(ErrorMessage = "Un avatar est obligatoire")]
        public IFormFile Avatar { get; set; }

    }

}