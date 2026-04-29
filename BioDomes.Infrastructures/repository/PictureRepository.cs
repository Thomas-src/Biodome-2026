using BioDomes.Domains.Repository;

namespace BioDomes.Infrastructures.repository;

public class PictureRepository : IPictureRepository
{
    private const string SPECIES_PATH = "species";
    private const string BIOMES_PATH = "biodomes";
    private const string EQUIPEMENTS_PATH = "equipements";
    
    public string Save(string fileName, Stream fileStream, string imageType)
    {
        string path;
        
        var folder = imageType.ToLower() switch
        {
            "biodomes" => BIOMES_PATH,
            "species" => SPECIES_PATH,
            _ => EQUIPEMENTS_PATH
        };


        var newFileName = Guid.NewGuid() + Path.GetExtension(fileName);

        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folder);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(folderPath, newFileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        fileStream.CopyTo(stream);

        return newFileName;
    }

    public void Delete(string fileName, string imageType)
    {
        var folder = imageType.ToLower() switch
        {
            "biodomes" => BIOMES_PATH,
            "species" => SPECIES_PATH,
            _ => EQUIPEMENTS_PATH
        };

        var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "images",
            folder,
            fileName
        );

        if (File.Exists(path))
        {
            File.Delete(path);
        }
           
    }
}