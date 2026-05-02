using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Constants;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services.interfaces;
using BioDomes.Web.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Equipements;

[Authorize(Roles = AppRoles.Admin + "," + AppRoles.Research)]
public class Create : PageModel
{
    private readonly IEquipementService _service;
    private readonly IConsumptionRepository _consumptionRepository;
    private readonly IProductionRepository _productionRepository;

    public Create(IEquipementService service, IConsumptionRepository consumptionRepository,
        IProductionRepository productionRepository)
    {
        _service = service;
        _consumptionRepository = consumptionRepository;
        _productionRepository = productionRepository;
    }

    [BindProperty] public InputModel Equipement { get; set; }

    [BindProperty] public IReadOnlyList<Consumptions>? Consumptions { get; set; }

    [BindProperty] public IReadOnlyList<Production> Productions { get; set; }

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


        var allowed = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };

        if (!allowed.Contains(Equipement.Avatar.ContentType))
        {
            ModelState.AddModelError("Equipement.Avatar", "Seuls les formats JPG, PNG, GIF et WebP sont acceptés.");
        }

        if (Equipement.Avatar.Length > 2 * 1024 * 1024)
        {
            ModelState.AddModelError("Equipement.Avatar", "L'avatar ne peut pas dépasser 2 Mo.");
        }

        var cons = GetCons(out var prod);

        var created = new Equipement(
            Equipement.Name,
            cons,
            prod,
            Equipement.Avatar.FileName,
            false,
            User.Identity.Name
        );

        try
        {
            _service.Save(created, Equipement.Avatar.OpenReadStream());
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", $"{e.Message}");
            LoadSelect();
            return Page();
        }

        TempData["SuccessMessage"] = $"L'équipement {Equipement.Name} a été ajoutée avec succès !";
        return RedirectToPage("Equipements");
    }

    private Consumptions GetCons(out Production production)
    {
        Consumptions consumptions;
        production = Enum.Parse<Production>(Equipement.Production);

        if (Enum.TryParse<Consumptions>(Equipement.Consumption, out var consumption))
        {
            consumptions = consumption;
        }
        else
        {
            consumptions = Domains.Core.Equipements.Consumptions.None;
        }

        return consumptions;
    }

    private void LoadSelect()
    {
        Consumptions = _consumptionRepository.GetAllConsumptions();
        Productions = _productionRepository.GetAllProductions();
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$",
            ErrorMessage = "Le nom ne peut contenir que des lettres, espaces ou tirets")]
        public string Name { get; set; }

        public string? Consumption { get; set; }

        [Required(ErrorMessage = "La production est obligatoire")]
        public string Production { get; set; }

        [Required(ErrorMessage = "Une image est obligatoire")]
        [IsImageValid]
        public IFormFile Avatar { get; set; }
    }
}