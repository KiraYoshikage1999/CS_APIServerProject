using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;

namespace CS_APIServerProject.Services
{
    public interface IFileStorage
    {
        Task<string> SaveProductImageAsync(IFormFile file, CancellationToken ct = default);
        Task DeleteAsync(string? relativePath = null, CancellationToken ct = default);
    }
}
