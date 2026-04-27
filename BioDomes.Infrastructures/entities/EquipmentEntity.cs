namespace BioDomes.Infrastructures.entities;

public class EquipmentEntity
{
    public int IdEquipment { get; set; }
    public string Name { get; set; }
    
    // Production et consommation
    public ProductionEntity Production { get; set; }
    public int ProductionId { get; set; }
    
    public ConsumptionEntity? Consumption { get; set; }
    public int? ConsumptionId { get; set; }
    public string Avatar { get; set; } = string.Empty;
    public bool Visible { get; set; }

// Lien avec User
    public string? UserId { get; set; }
    public UserEntity? User { get; set; }

    // Lien avec Biodomes
    public IList<BiodomeEntity> Biodomes { get; set; } = new List<BiodomeEntity>();
}