using CS_APIServerProject.DTO;
using CS_APIServerProject.Models;
using Microsoft.AspNetCore.Mvc;
namespace CS_APIServerProject.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts(CancellationToken cancellationToken = default);
        Task<Product> GetProductById(Guid id, CancellationToken cancellationToken = default);

        Task<ActionResult<ProductCreateDTO>> AddAsync(ProductCreateDTO product, CancellationToken cancellationToken = default);

        Task DeleteAsync(Product product);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
