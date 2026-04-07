namespace BioDomes.Infrastructures.entities;

public class Biodomes
{
    public string idBiodomes { get; set; }
    public string Name { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public string Avatar  { get; set; } = string.Empty;
}