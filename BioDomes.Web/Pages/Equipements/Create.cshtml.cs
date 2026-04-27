using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Core.Equipements;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Equipements;

[Authorize]
public class Create : PageModel
{
    private readonly IEquipementService _service;
    private readonly IConsumptionRepository _consumptionRepository;
    private readonly IProductionRepository _productionRepository;

    public Create(IEquipementService service,  IConsumptionRepository consumptionRepository, IProductionRepository productionRepository)
    {
        _service = service;
        _consumptionRepository = consumptionRepository;
        _productionRepository = productionRepository;
    }

    [BindProperty]
    public InputModel Equipement { get; set; }

    [BindProperty]
    public IReadOnlyList<Consumptions>? Consumptions { get; set; }
    
    [BindProperty]
    public IReadOnlyList<Production> Productions { get; set; }
    
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
        
        return  RedirectToPage("Equipements");
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
        public string Name{get;set;}
        
        public string? Consumption { get; set; }
        
        [Required(ErrorMessage = "La production est obligatoire")]
        public string Production { get; set; }
        
        [Required(ErrorMessage = "Une image est obligatoire")]
        public IFormFile Avatar { get; set; }
    }
}