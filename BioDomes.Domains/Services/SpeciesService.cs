using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services;

public class SpeciesService : ISpeciesService
{
    public IReadOnlyList<Species> FilterOwnerSpecies(string ownerSpecies, IReadOnlyList<Species> species)
    {
        IList<Species> result = new List<Species>();

        foreach (var sp in species)
        {
            if (sp.OwnerName != null)
            {
                if (sp.OwnerName.ToUpper() == ownerSpecies.ToUpper())
                {
                    result.Add(sp);
                }
            }
        }

        return result.AsReadOnly();
    }

    public IReadOnlyList<Species> FilterSpecies(string ownerSpecies, IReadOnlyList<Species> species)
    {
        IList<Species> result = new List<Species>();

        foreach (var sp in species)
        {
            if (sp.IsVisible)
            {
                if (sp.OwnerName == null)
                {
                    result.Add(sp);
                } else if (sp.OwnerName.ToUpper() != ownerSpecies.ToUpper())
                {
                    result.Add(sp);
                }
            }
        }

        return result.AsReadOnly();
    }
}