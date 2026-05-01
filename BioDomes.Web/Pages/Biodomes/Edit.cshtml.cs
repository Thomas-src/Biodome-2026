using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Constants;
using BioDomes.Domains.Core;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Biodomes;

[Authorize(Roles = AppRoles.Admin + "," + AppRoles.Research)]
public class Edit : PageModel
{
    private readonly IBiodomeService _service;

    [BindProperty] public InputModel Biome { get; set; }
    public Biodome Biodome { get; set; }

    public Edit(IBiodomeService service)
    {
        _service = service;
    }

    public void OnGet(int id)
    {
        Biodome = _service.GetBiodomeById(id);

        Biome = new InputModel
        {
            Name = Biodome.Nom,
            temperature = Biodome.Temperature,
            humidite = Biodome.Humidite,
            AvatarPath = Biodome.Image
        };
    }

    public IActionResult OnPost(int id)
    {
        Biodome = _service.GetBiodomeById(id);


        if (!ModelState.IsValid)
        {
            return Page();
        }

        double humiditeMax = _service.CalculateHumiditeMax(Biome.temperature.Value);

        if (Biome.humidite.Value > humiditeMax)
        {
            ModelState.AddModelError("Biome.humidite",
                $"Un biome avec une température de {Biome.temperature.Value}° peut avoir une humidité max de {humiditeMax} g/m³");
            return Page();
        }

        string fileName;

        if (Biome.Avatar != null)
        {
            fileName = Biome.Avatar.FileName;
        }
        else
        {
            fileName = Biodome.Image;
        }

        Biodome created = new Biodome(
            Biodome.Id,
            Biome.Name,
            Biome.temperature.Value,
            Biome.humidite.Value,
            fileName,
            Biodome.Species,
            Biodome.Equipements,
            User.Identity.Name
        );

        if (Biome.Avatar != null)
        {
            _service.Update(created, Biome.Avatar.FileName, Biome.Avatar.OpenReadStream());
        }
        else
        {
            _service.Update(created);
        }


        TempData["SuccessMessage"] = $"Le {Biome.Name} a été modifié avec succès !";
        return RedirectToPage("/Biodomes/Details", new { id = Biodome.Id });
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

// Un avatar est obligatoire à la création
        public IFormFile? Avatar { get; set; }

        public string? AvatarPath { get; set; }
    }
}