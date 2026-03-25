using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IList<Biodome> _biodomes;

    public IList<Biodome> Biodomes
    {
        get { return _biodomes; }
        set { _biodomes = value; }
    }

    public void OnGet()
    {
        IRepository repository = new SqlRepository();
        _biodomes = repository.GetBestThreeBiodome();
    }
}