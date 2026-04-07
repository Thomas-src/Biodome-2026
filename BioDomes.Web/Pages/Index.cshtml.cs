using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IBiodomeRepository _repository;

    public IndexModel(IBiodomeRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyList<Biodome> Biodomes { get; set; } = new List<Biodome>();

    public void OnGet()
    {
        Biodomes = _repository.GetBestThreeBiodome();
    }
}