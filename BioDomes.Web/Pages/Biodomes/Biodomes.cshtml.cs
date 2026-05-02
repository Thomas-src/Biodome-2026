using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Services;
using BioDomes.Domains.Services.interfaces;
using BioDomes.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Biodomes;

public class Biodomes : PageModel
{
    private readonly IBiodomeService _service;

    public Biodomes(IBiodomeService service)
    {
        _service = service;
    }

    public IReadOnlyList<Biodome> _allbiodomes;

    public IReadOnlyList<Biodome> AllBiodomes
    {
        get { return _allbiodomes; }
        set { _allbiodomes = value; }
    }
    
    [BindProperty(SupportsGet = true)]
    public double? SearchTemperatureMin { get; set; }

    [BindProperty(SupportsGet = true)]
    public double? SearchTemperatureMax { get; set; }

    [BindProperty(SupportsGet = true)]
    public double? SearchHumiditeMin { get; set; }

    [BindProperty(SupportsGet = true)]
    public double? SearchHumiditeMax { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SearchEquipement { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SearchEspece { get; set; }
    
    public void OnGet()
    {
        try
        {
            AllBiodomes = _service.GetAllStableBiodomes();

            AllBiodomes = _service.SearchBiodomes(
                SearchTemperatureMin,
                SearchTemperatureMax,
                SearchHumiditeMin,
                SearchHumiditeMax,
                SearchEquipement,
                SearchEspece
            );
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
        }

    }
}