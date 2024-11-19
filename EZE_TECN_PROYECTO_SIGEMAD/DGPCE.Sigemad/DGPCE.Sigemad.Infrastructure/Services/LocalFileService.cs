using DGPCE.Sigemad.Application.Contracts.Files;
using Microsoft.Extensions.Configuration;

namespace DGPCE.Sigemad.Infrastructure.Services;
public class LocalFileService : IFileService
{
    private readonly string _folderBase;

    public LocalFileService(IConfiguration configuration)
    {
        _folderBase = configuration["Archivos:DirectorioBase"];
    }

    public async Task DeleteAsync(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public async Task<Stream> GetFileAsync(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException();
        return new FileStream(filePath, FileMode.Open, FileAccess.Read);
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string fileCategory)
    {
        // Generar la ruta de almacenamiento
        string year = DateTime.UtcNow.Year.ToString();
        string month = DateTime.UtcNow.Month.ToString("D2");

        var folderPath = Path.Combine(_folderBase, fileCategory, year, month);
        var filePath = Path.Combine(folderPath, fileName);

        // Crear directorios si no existen
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Guardar el archivo
        using var fileStreamToSave = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(fileStreamToSave);

        return filePath;
    }
}
