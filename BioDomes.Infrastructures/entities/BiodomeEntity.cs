namespace BioDomes.Infrastructures.entities;

public class BiodomeEntity
{
    public int idBiodomes { get; set; }
    public string Name { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public string Avatar  { get; set; } = string.Empty;

    // Lien avec User
    public string? userId { get; set; }
    public UserEntity? user { get; set; }

    // Lien avec BiodomeSpecies
    
    public IList<BiodomeSpeciesEntity> BiodomeSpecies { get; set; } = new List<BiodomeSpeciesEntity>();
    // Lien avec Equipement
    public IList<EquipmentEntity> Equipment { get; set; } = new List<EquipmentEntity>();
}