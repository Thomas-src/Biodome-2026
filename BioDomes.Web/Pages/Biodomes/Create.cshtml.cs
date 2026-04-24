using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Biodomes;

public class Create : PageModel
{
    [BindProperty]
    public InputModel Biome { get; set; }

    public void OnGet()
    {
        
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$",
            ErrorMessage = "Le nom ne peut contenir que des lettres, espaces ou tirets")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La temperature est obligatoire")]
        [Range(-100, 100, ErrorMessage = "La taille doit être entre -100 et 100")]
        public double temperature { get; set; }

        [Required(ErrorMessage = "L'humidité est obligatoire")]
        [Range(-100, 100, ErrorMessage = "L'humitidé doit être entre -100 et 100")]
        public double humidite { get; set; }

        [Required(ErrorMessage = "Un avatar est obligatoire")]
        public IFormFile Avatar { get; set; }

    }

}