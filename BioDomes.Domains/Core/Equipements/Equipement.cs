namespace BioDomes.Domains.Core.Equipements;

public class Equipement
{
    public Equipement(string name, Consumptions consumption, Production production, string avatar, bool visibility, string ownerName)
    {
        this.Name = name;
        this.Consumption = consumption;
        this.Production = production;
        this.Avatar = avatar;
        this.Visibility = visibility;
        this.OwnerName = ownerName;
    }
    
    public string Name { get; }
    public Consumptions Consumption { get; }
    public Production Production { get; }
    public string Avatar { get; set; }

    public bool Visibility { get; set; }

    public string OwnerName { get; }

    public override bool Equals(object other)
    {
        if (other == null || !(other is Equipement))
        {
            return false;
        }
        
        Equipement otherEquipement = (Equipement)other;

        if (otherEquipement.Name == this.Name && otherEquipement.Consumption == this.Consumption && otherEquipement.Production == this.Production)
        {
            return true;
        }

        return false;
    }
}