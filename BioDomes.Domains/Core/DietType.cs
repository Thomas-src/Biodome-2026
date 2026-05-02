namespace BioDomes.Domains.Core;

public enum DietType
{
    Photosynthèse,
    Carnivore,
    Omnivore,
    Coprophage,
    Herbivore,
    Détritivores,
    Hétérotrophie,
    Coprophages
}

public static class DietTypeExtensions
{
    public static int GetTrophicLevel(this DietType diet) => diet switch
    {
        DietType.Photosynthèse => 0,
        DietType.Herbivore or DietType.Détritivores or DietType.Coprophage or DietType.Coprophages => 1,
        DietType.Carnivore or DietType.Omnivore or DietType.Hétérotrophie => 2,
        _ => 3
    };
    
    public static string GetColor(this DietType diet) => diet switch
    {
        DietType.Photosynthèse => "#4ade80",
        DietType.Herbivore     => "#60a5fa",
        DietType.Carnivore     => "#f87171",
        DietType.Omnivore      => "#fb923c",
        DietType.Détritivores  => "#a78bfa",
        DietType.Coprophage    => "#a78bfa",
        DietType.Coprophages   => "#a78bfa",
        DietType.Hétérotrophie => "#e879f9",
        _                      => "#FFFFFF"
    };
}