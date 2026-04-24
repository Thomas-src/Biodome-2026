using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Services;
using BioDomes.Infrastructures;
using Microsoft.AspNetCore.Authorization;
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

    public void OnGet()
    {

        AllBiodomes = _service.GetAllBiodomes();

    }
}