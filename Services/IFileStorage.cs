using Microsoft.AspNetCore.Http;

namespace CS_APIServerProject.Services
{
    public interface IFileStorage
    {
        Task<string> SaveProductImageAsync(IFormFile file, CancellationToken ct = default);
        Task DeleteAsync(string? relativePath = null, CancellationToken ct = default);


        //Special method for encode file Image into string code that i gonna give later to object and then decode.
        Task<string> Base64Encode(string? relativePath = null, CancellationToken ct = default);
        //Decode base64 string to image file and return relative path
        Task<string> Base64Decode(string base64, CancellationToken ct = default);


    }
}
