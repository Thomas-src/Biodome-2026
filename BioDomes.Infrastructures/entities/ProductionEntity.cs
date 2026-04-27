namespace BioDomes.Infrastructures.entities;

public class ProductionEntity
{
    public int ProductionId { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public IList<EquipmentEntity> Equipments { get; set; } = new List<EquipmentEntity>();
}