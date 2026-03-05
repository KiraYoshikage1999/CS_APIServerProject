using CS_APIServerProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;

namespace CS_APIServerProject.Services
{
    public interface IFileStorage
    {
        Task<string> SaveProductImageAsync(IFormFile file, CancellationToken ct = default);
        Task DeleteAsync(string? relativePath = null, CancellationToken ct = default);


        //Special method for encode file Image into string code that i gonna give later to object and then decode.
        Task<string> Base64Encode(string? relativePath = null, CancellationToken ct = default);
        //Decode to Image
        Task<string> Base64Decode(Product product, CancellationToken ct = default);


    }
}
