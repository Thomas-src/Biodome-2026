namespace BioDomes.Domains.Repository;

public interface IPictureRepository
{
    
    /**
     * Enregistre une image dans une directory en fonctione du type de l'image (species, biodomes etc...)
     *
     * retourne un string du nom du fichier correspondant
     */
    public string Save(string fileName, Stream fileStream, string imageType);
}