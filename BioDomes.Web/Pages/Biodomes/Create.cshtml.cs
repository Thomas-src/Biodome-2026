using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Constants;
using BioDomes.Domains.Core;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using BioDomes.Web.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Web.Pages.Biodomes;

[ValidateAntiForgeryToken]
[Authorize(Roles = AppRoles.Admin + "," + AppRoles.Research)]
public class Create : PageModel
{
    private readonly IBiodomeService _service;

    public Create(IBiodomeService service)
    {
        _service = service;
    }

    [BindProperty] public InputModel Biome { get; set; }

    public IActionResult OnPost()
    {
        if (_service.BiodomeExists(Biome.Name.ToSlug()))
        {
            ModelState.AddModelError("Biome.Name", "Ce nom de biodome est déjà réservé");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var created = new Biodome(
            Biome.Name,
            Biome.temperature,
            Biome.humidite,
            Biome.Avatar.FileName,
            new List<Domains.Core.Species>(),
            new List<Equipement>(),
            User.Identity.Name,
            Biome.Name.ToSlug()
        );
        try

        {
            _service.Save(created, Biome.Avatar.OpenReadStream());
        }
        catch
            (ArgumentException e)
        {
            ModelState.AddModelError("Biome.humidite", e.Message);
            return Page();
        }

        TempData["SuccessMessage"] = $"L'espèce {Biome.Name} a été ajoutée avec succès !";
        return RedirectToPage("UserBiodomes");
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$",
            ErrorMessage = "Le nom ne peut contenir que des lettres, espaces ou tirets")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "La temperature est obligatoire")]
        [Range(-100, 100, ErrorMessage = "La taille doit être entre -100 et 100")]
        public required double temperature { get; set; }

        [Required(ErrorMessage = "L'humidité est obligatoire")]
        [Range(-100, 100, ErrorMessage = "L'humitidé doit être entre -100 et 100")]
        public required double humidite { get; set; }

        [Required(ErrorMessage = "Un avatar est obligatoire")]
        [IsImageValid]
        public required IFormFile Avatar { get; set; }
    }
}