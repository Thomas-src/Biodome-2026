namespace BioDomes.Infrastructures.entities;

public class EquipmentEntity
{
    public string IdEquipment { get; set; }
    public string Name { get; set; }
    private string Production { get; set; } = string.Empty;
    public string Consumption { get; set; } = string.Empty;
    public string Avatar  { get; set; } = string.Empty;

    // Lien avec User
    public string? UserId { get; set; }
    public UserEntity? User { get; set; }

    // Lien avec Biodomes
    public IList<BiodomeEntity> Biodomes { get; set; } = new List<BiodomeEntity>();
}