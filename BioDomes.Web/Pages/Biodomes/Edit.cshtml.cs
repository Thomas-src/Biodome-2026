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

namespace BioDomes.Web.Pages.Biodomes;

[Authorize(Roles = AppRoles.Admin + "," + AppRoles.Research)]
[ValidateAntiForgeryToken]
public class Edit : PageModel
{
    private readonly IBiodomeService _service;

    [BindProperty] public InputModel Biome { get; set; }
    public Biodome Biodome { get; set; }

    public Edit(IBiodomeService service)
    {
        _service = service;
    }

    public IActionResult OnGet(string id)
    {
        var biodome = _service.GetBiodomeBySlug(id);
        if (biodome == null)
        {
            return NotFound();
        }
        Biodome = biodome;

        Biome = new InputModel
        {
            Name = Biodome.Nom,
            Temperature = Biodome.Temperature,
            Humidite = Biodome.Humidite,
            AvatarPath = Biodome.Image
        };
        return Page();
    }

    public IActionResult OnPost(string id)
    {
        var biodome = _service.GetBiodomeBySlug(id);
        if (biodome == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            Biodome = biodome;
            return Page();
        }
        Biodome = biodome;
        // TODO refactor ca dans le service + ajouter un bouton delete
        var fileName = Biome.Avatar != null ? Biome.Avatar.FileName : Biodome.Image;

        var created = new Biodome(
            Biome.Name,
            Biome.Temperature,
            Biome.Humidite,
            fileName,
            Biodome.Species,
            Biodome.Equipements,
            User.Identity.Name,
            Biodome.Nom.ToSlug()
        );

        try
        {
            if (Biome.Avatar != null)
            {
                _service.Update(created, Biome.Avatar.FileName, Biome.Avatar.OpenReadStream());
            }
            else
            {
                _service.Update(created);
            }
        }
        catch (ArgumentException e)
        {
            ModelState.AddModelError("Biome.humidite", e.Message);
            return Page();
        }
        TempData["SuccessMessage"] = $"Le {Biome.Name} a été modifié avec succès !";
        return RedirectToPage("/biodomes/details", new { id = Biodome.Slug });
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$",
            ErrorMessage = "Le nom ne peut contenir que des lettres, espaces ou tirets")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "La temperature est obligatoire")]
        [Range(-100, 100, ErrorMessage = "La taille doit être entre -100 et 100")]
        public required double Temperature { get; set; }

        [Required(ErrorMessage = "L'humidité est obligatoire")]
        [Range(-100, 100, ErrorMessage = "L'humitidé doit être entre -100 et 100")]
        public required double Humidite { get; set; }
        [IsImageValid]
        public IFormFile? Avatar { get; set; }

        public string? AvatarPath { get; set; }
    }
}