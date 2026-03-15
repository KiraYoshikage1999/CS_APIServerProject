using CS_APIServerProject.DTO;
using CS_APIServerProject.Models;
using Microsoft.AspNetCore.Mvc;
namespace CS_APIServerProject.Repository
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllProducts(CancellationToken cancellationToken = default);
        public Task<Product> GetProductById(Guid id, CancellationToken cancellationToken = default);

        public Task<ActionResult<Product>> AddAsync(ProductCreateDTO product, CancellationToken cancellationToken = default);

        public Task<Task> DeleteAsync(Guid Id, CancellationToken ct = default);// must send Guid Id and delete by id
       

    }
}
