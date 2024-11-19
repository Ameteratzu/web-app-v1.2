namespace DGPCE.Sigemad.Application.Contracts.Files;
public interface IFileService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string fileCategory);
    Task DeleteAsync(string filePath);
    Task<Stream> GetFileAsync(string filePath);
}
