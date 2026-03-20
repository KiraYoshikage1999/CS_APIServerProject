using CS_APIServerProject.Utils;
using CS_APIServerProject.Data;
using CS_APIServerProject.DTO;
using CS_APIServerProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
namespace CS_APIServerProject.Services
{
    public class FileStorage : IFileStorage
    {

        private readonly DataBaseContext _db;
        // removed AutoMapper
        //private readonly IFileStorage _fs;
        // Use IWebHostEnvironment instead of referencing controllers to avoid circular DI
        private readonly IWebHostEnvironment _env;


        public FileStorage(DataBaseContext db, IWebHostEnvironment env /*, IFileStorage fs */)
        {
            _db = db;
            _env = env;
        }

        //Like i understand this is special set of types for file that gonna be used for his combining or checking is that exists.
        //StringComparer.OrdinalIngoreCase is ingnoring style of Case in which wrote these types.
        private static readonly HashSet<string> AllowedExt = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg" , ".png" ,"webp"
        };

        //Valuable of max bytes per file (Image)
        private const long MaxBytes = 3 * 1024 * 1024;

        //Provides info bout this project and host (injected)

        //Method to Delete async some file.
        public Task DeleteAsync(string? relativePath = null, CancellationToken ct = default)
        {
            //Checking for the presence of something
            if (string.IsNullOrWhiteSpace(relativePath)) { return Task.CompletedTask; }

            //Just getting full path of host.
            var webRoot = _env.WebRootPath ?? "wwwroot";
            //Replacing '/' symbols with just separetor in string.
            var trimmed = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            //Getting full path
            var fullpath = Path.Combine(webRoot, trimmed);

            //Checking if file exists and than deleting.
            if(File.Exists(fullpath)) { File.Delete(fullpath); }



            return Task.CompletedTask;
        }

        public async Task<string> SaveProductImageAsync(IFormFile file, CancellationToken ct = default)
        {
            if (file == null || file.Length == 0) {throw new ArgumentNullException(nameof(file));}

            if(file.Length > MaxBytes) {throw new ArgumentOutOfRangeException(nameof(file));}

            //var ext = _env.WebRootPath ?? "wwwroot";
            //var die = Path.Combine()

            var ext = Path.GetExtension(file.FileName);
            if(string.IsNullOrWhiteSpace(ext) || !AllowedExt.Contains(ext)) 
            { throw  new ArgumentNullException(nameof(ext));}

            var webRoot = "wwwroot";
            var dir = Path.Combine(webRoot, "uploads", "Product");
            Directory.CreateDirectory(dir);

            var name = $"{Guid.NewGuid():N}{ext}";
            var fullPath= Path.Combine(dir, name);

            await using var fs = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fs, ct);

            return $"/uploads/products/{name}";
        }

        public  Task<string> Base64Encode(string? relativePath = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return Task.FromResult(string.Empty);
            }

            // Just getting full path of host.
            var webRoot = _env?.WebRootPath ?? "wwwroot";
            // Replacing '/' symbols with OS separator
            var trimmed = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullpath = Path.Combine(webRoot, trimmed);

            if (!File.Exists(fullpath))
            {
                return Task.FromResult(string.Empty);
            }

            byte[] imageBytes = File.ReadAllBytes(fullpath);
            string convertedBytes = Convert.ToBase64String(imageBytes);

            return Task.FromResult(convertedBytes);
        }   

        public Task<string> Base64Decode(string base64, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return Task.FromResult(string.Empty);

            byte[] imageBytes;
            try
            {
                imageBytes = Convert.FromBase64String(base64);
            }
            catch (FormatException)
            {
                return Task.FromResult(string.Empty);
            }

            var webRoot = _env?.WebRootPath ?? "wwwroot";
            var dir = Path.Combine(webRoot, "uploads", "products");
            Directory.CreateDirectory(dir);

            var name = $"{Guid.NewGuid():N}.png";
            var fullPath = Path.Combine(dir, name);

            File.WriteAllBytes(fullPath, imageBytes);

            // return relative path
            var relative = $"/uploads/products/{name}";
            return Task.FromResult(relative);
        }
    }
}
