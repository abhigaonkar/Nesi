using Microsoft.AspNetCore.Http;

namespace Nesi.Application.Services;

/// <summary>
/// Local file storage implementation (for development)
/// In production, this would be replaced with Azure Blob Storage, AWS S3, etc.
/// </summary>
public class LocalFileStorageService : IFileStorageService
{
    private readonly string _storagePath;
    private readonly string _baseUrl;

    public LocalFileStorageService(string storagePath, string baseUrl)
    {
        _storagePath = storagePath;
        _baseUrl = baseUrl;
        
        // Ensure storage directory exists
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    public async Task<string> UploadFileAsync(IFormFile file, string path, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty", nameof(file));

        // Create directory if it doesn't exist
        var fullPath = Path.Combine(_storagePath, path);
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        // Generate unique filename
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(fullPath, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        // Return URL
        var relativePath = Path.Combine(path, fileName).Replace("\\", "/");
        return $"{_baseUrl}/{relativePath}";
    }

    public Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        // Extract relative path from URL
        var relativePath = fileUrl.Replace(_baseUrl + "/", "");
        var filePath = Path.Combine(_storagePath, relativePath);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }

    public Task<string> GetDownloadUrlAsync(string fileUrl, int expirationMinutes = 60, CancellationToken cancellationToken = default)
    {
        // For local storage, just return the same URL
        // In production with cloud storage, this would generate a signed URL
        return Task.FromResult(fileUrl);
    }
}
