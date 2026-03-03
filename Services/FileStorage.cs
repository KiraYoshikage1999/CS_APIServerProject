
namespace CS_APIServerProject.Services
{
    public class FileStorage : IFileStorage
    {
        private static readonly HashSet<string> AllowedExt = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg" , ".png" ,"webp"
        };

        private const long MaxBytes = 3 * 1024 * 1024;

        private IWebHostEnvironment _env;
        public Task DeleteAsync(string? relativePath = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) { return Task.CompletedTask; }

            var webRoot = _env.WebRootPath ?? "wwwroot";
            var trimmed = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullpath = Path.Combine(webRoot, trimmed);

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
    }
}
