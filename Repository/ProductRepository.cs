using CS_APIServerProject.Utils;
using CS_APIServerProject.Data;
using CS_APIServerProject.DTO;
using CS_APIServerProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CS_APIServerProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using CS_APIServerProject.Repository;

namespace CS_APIServerProject.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataBaseContext _db;
        // removed AutoMapper
        private readonly IFileStorage _fs;
        public ProductRepository(DataBaseContext db , IFileStorage fs)
        {
            _db = db;
            _fs = fs;
        }

        /*
         Pseudocode / Plan:
         - Validate input product is not null.
         - If product.Id is Guid.Empty, assign a new Guid.
         - Add the product to the DbContext's Products set using AddAsync and the provided cancellation token.
         - Do NOT call SaveChangesAsync here (controller's CreateAsync calls SaveChangesAsync separately).
         - Return when the add operation completes.
        */
        async Task<ActionResult<Product>> IProductRepository.AddAsync(ProductCreateDTO product, CancellationToken cancellationToken)
        {
            var entity = ManualMapper.ToProduct(product);
            entity.Id = Guid.NewGuid();
            entity.Characteristics ??= new Characteristics();

            //if (product.Image != null && product.Image.Length > 0)
            //{
            //    var imagePath = await _fs.SaveProductImageAsync(product.Image, cancellationToken);
            //    entity.ImagePath = imagePath;
            //}
            _db.Products.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            //var result = _maper.Map<Product>(entity);
            var result = new Product
            {
                Id = entity.Id,
                Brand = entity.Brand,
                Model = entity.Model,
                Description = entity.Description,
                Price = entity.Price,
                Quanity = entity.Quanity,
                //FK_Salesman = entity.FK_Salesman,
                Currency = entity.Currency,
                Characteristics = entity.Characteristics,
                imageCode = entity.imageCode
            };
            return new Product
            {
                Id = result.Id,
                Brand = result.Brand,
                Model = result.Model,
                Description = result.Description,
                Price = result.Price,
                Quanity = result.Quanity,
                //FK_Salesman = result.FK_Salesman,
                Currency = result.Currency,
                Characteristics = result.Characteristics,
                imageCode = result.imageCode
            };
        }

        public async Task<Task> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var product = _db.Products.FindAsync(id, ct);  
            _db.Remove(product);
            await _db.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public async Task<List<Product>> GetAllProducts(CancellationToken ct = default)
        {
            var list = await _db.Products.AsNoTracking().ToListAsync(ct);
            if (list != null || list.Count > 0) return list; //return await Task.FromResult(list);
            else throw new ArgumentNullException(nameof(list), "List is null or empty");
        }

        public async Task<Product> GetProductById(Guid id, CancellationToken ct = default)
        {
            var item = await _db.Products.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

                if (item != null) return item; //(Product)Task.FromException(new Exception("Item null or doesn't exist")); }
                else throw new ArgumentNullException(nameof(item), "Item is null or doesn't exist");
           
        }
    }
}
