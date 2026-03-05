using AutoMapper;
using CS_APIServerProject.Controllers;
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
        private readonly IMapper _maper;
        private readonly IFileStorage _fs;
        //Temporaly i connect Product Contoller but in future I will change it to Product Repository.
        private readonly ProductController _controller;


        public FileStorage(DataBaseContext db, IMapper maper, IFileStorage fs , ProductController controller)
        {
            _maper = maper;
            _db = db;
            _fs = fs;
            _controller = controller;
        }

        //Like i understand this is special set of types for file that gonna be used for his combining or checking is that exists.
        //StringComparer.OrdinalIngoreCase is ingnoring style of Case in which wrote these types.
        private static readonly HashSet<string> AllowedExt = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg" , ".png" ,"webp"
        };

        //Valuable of max bytes per file (Image)
        private const long MaxBytes = 3 * 1024 * 1024;

        //Provides info bout this project and host
        private IWebHostEnvironment _env;

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
            if (string.IsNullOrWhiteSpace(relativePath)) { return (Task<string>)Task.CompletedTask; }

            //Just getting full path of host.
            var webRoot = _env.WebRootPath ?? "wwwroot";
            //Replacing '/' symbols with just separetor in string.
            var trimmed = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            //Getting full path
            var fullpath = Path.Combine(webRoot, trimmed);

            byte[] imageBytes = File.ReadAllBytes(fullpath);
            string convertedBytes = Convert.ToBase64String(imageBytes);

            return Task<string>.FromResult(convertedBytes);
        }   

        public Task<string> Base64Decode([FromBody] Product product, CancellationToken ct = default)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            // Call controller method (may return ActionResult<Product>, Task<ActionResult<Product>> or Product)
            var rawResult = _controller.GetById(product.Id, ct);

            Product? productFromController = null;

            productFromController = rawResult;

            //string imageBytes = Convert.FromBase64String(productFromController);
            

            // Continue implementation: decode base64 from productFromController as needed.
            // Example placeholder (adjust property name accordingly):
            // byte[] imageBytes = Convert.FromBase64String(productFromController.ImageBase64);
            // return Task.FromResult(Encoding.UTF8.GetString(imageBytes));
            throw new NotImplementedException();
        }
    }
}
