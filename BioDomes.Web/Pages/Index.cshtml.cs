using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IBiodomeService _service;

    public IndexModel(IBiodomeService service)
    {
        _service = service;
    }

    public IReadOnlyList<Biodome> Biodomes { get; set; } = new List<Biodome>();

    public void OnGet()
    {
        Biodomes = _service.GetBestThreeBiodome();
    }
}