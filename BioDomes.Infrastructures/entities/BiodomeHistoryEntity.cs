namespace BioDomes.Infrastructures.entities;

public class BiodomeHistoryEntity
{
    public int Id { get; set; }
    public int BiodomeId { get; set; }
    public BiodomeEntity Biodome { get; set; }
    public int SpeciesId { get; set; }
    public SpeciesEntity Species { get; set; }
    public int Delta { get; set; }
    public DateTime UpdateAt { get; set; }
    
}