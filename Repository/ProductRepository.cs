using AutoMapper;
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

namespace CS_APIServerProject.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataBaseContext _db;
        private readonly IMapper _maper;
        private readonly IFileStorage _fs;
        public ProductRepository(DataBaseContext db , IMapper maper, IFileStorage fs)
        {
            _maper = maper;
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
        async Task<ActionResult<ProductCreateDTO>> IProductRepository.AddAsync(ProductCreateDTO product, CancellationToken cancellationToken)
        {
            //if (!ModelState.IsValid)
            //{
            //    return ValidationProblem(ModelState);
            //}

            var entity = _maper.Map<Product>(product);

            entity.Id = Guid.NewGuid();
            entity.Characteristics ??= new Characteristics();

            //if (product.Image != null && product.Image.Length > 0)
            //{
            //    var imagePath = await _fs.SaveProductImageAsync(product.Image, cancellationToken);
            //    entity.ImagePath = imagePath;
            //}
            _db.Products.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            var result = _maper.Map<ProductReadDTO>(entity);
            //return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
            return new ProductCreateDTO
            {
                
                Brand = result.Brand,
                Model = result.Model,
                Description = result.Description,
                Price = result.Price,
                Quanity = result.Quanity,
                //FK_Salesman = result.FK_Salesman,
                Currency = result.Currency,
                Characteristics = result.Characteristics,
                ImageCode = result.ImageCode
            };
        }

        Task IProductRepository.DeleteAsync(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            _db.Products.Remove(product);
            return Task.CompletedTask;
        }

        Task<List<Product>> IProductRepository.GetAllProducts(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<Product> IProductRepository.GetProductById(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task IProductRepository.SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
