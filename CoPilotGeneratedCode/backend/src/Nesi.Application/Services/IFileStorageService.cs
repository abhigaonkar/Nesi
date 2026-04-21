using Microsoft.AspNetCore.Http;

namespace Nesi.Application.Services;

/// <summary>
/// Service for file storage operations
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Uploads a file to storage
    /// </summary>
    /// <param name="file">The file to upload</param>
    /// <param name="path">The storage path (e.g., "work-orders/123")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The URL of the uploaded file</returns>
    Task<string> UploadFileAsync(IFormFile file, string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a file from storage
    /// </summary>
    /// <param name="fileUrl">The URL of the file to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a temporary download URL for a file
    /// </summary>
    /// <param name="fileUrl">The file URL</param>
    /// <param name="expirationMinutes">URL expiration in minutes</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A temporary download URL</returns>
    Task<string> GetDownloadUrlAsync(string fileUrl, int expirationMinutes = 60, CancellationToken cancellationToken = default);
}
