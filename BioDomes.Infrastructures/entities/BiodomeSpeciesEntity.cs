namespace BioDomes.Infrastructures.entities;

public class BiodomeSpeciesEntity
{
    public int BiodomeId { get; set; }
    public BiodomeEntity Biodome { get; set; }

    public int SpeciesId { get; set; }
    public SpeciesEntity Species { get; set; }

    public int Quantity { get; set; }
}