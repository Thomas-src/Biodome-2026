namespace BioDomes.Infrastructures.entities;

public class ConsumptionEntity
{
    public int ConsumptionId { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public IList<EquipmentEntity> Equipments { get; set; } = new List<EquipmentEntity>();
}