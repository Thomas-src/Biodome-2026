namespace BioDomes.Domains.Core.Equipements;

public class Equipement
{
    private string name; 
    private Consumptions consumption;
    private Production production;
    private string avatar;
    private bool visibility;
    private string _ownerName; 

    public Equipement(string name, Consumptions consumption, Production production, string avatar, bool visibility, string ownerName)
    {
        this.name = name;
        this.consumption = consumption;
        this.production = production;
        this.avatar = avatar;
        this.visibility = visibility;
        this._ownerName = ownerName;
    }
    
    public string Name { get { return name; } }
    public Consumptions Consumption { get { return consumption; } }
    public Production Production { get { return production; } }
    public string Avatar { get { return avatar; } set { avatar = value; } }
    public bool Visibility { get { return visibility; } }
    
    public string OwnerName { get { return _ownerName; } }

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