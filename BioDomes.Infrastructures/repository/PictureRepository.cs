using BioDomes.Domains.Repository;

namespace BioDomes.Infrastructures.repository;

public class PictureRepository : IPictureRepository
{
    private const string SPECIES_PATH = "species";
    private const string BIOMES_PATH = "biodomes";
    
    public string Save(string fileName, Stream fileStream, string imageType)
    {
        string path;

        if (imageType.ToLower() == "biodomes")
        {
            path = BIOMES_PATH;
        }
        else
        {
            path = SPECIES_PATH;
        }
        
        
        var newFileName = Guid.NewGuid() + Path.GetExtension(fileName);

        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", path);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(folderPath, newFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            fileStream.CopyTo(stream);
        }

        return newFileName;
    }
    
}