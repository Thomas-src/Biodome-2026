namespace BioDomes.Infrastructures.entities;

public class Equipment
{
    public string IdEquipment { get; set; }
    public string Name { get; set; }
    private string Production { get; set; } = string.Empty;
    public string Consumption { get; set; } = string.Empty;
    public string Avatar  { get; set; } = string.Empty;
}