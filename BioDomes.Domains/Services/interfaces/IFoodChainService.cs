using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services.interfaces;

public interface IFoodChainService
{
    FoodChain Build(IReadOnlyList<Species> species);
}