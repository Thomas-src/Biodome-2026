namespace BioDomes.Infrastructures.entities;

public class BiodomeEntity
{
    public string idBiodomes { get; set; }
    public string Name { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public string Avatar  { get; set; } = string.Empty;

    // Lien avec User
    public string? userId { get; set; }
    public UserEntity? user { get; set; }

    // Lien avec Species
    public IList<SpeciesEntity> Species { get; set; } = new List<SpeciesEntity>();

    // Lien avec Equipement
    public IList<EquipmentEntity> Equipment { get; set; } = new List<EquipmentEntity>();
}