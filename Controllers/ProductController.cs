using CS_APIServerProject.Data;
using CS_APIServerProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CS_APIServerProject.DTO;
using AutoMapper;
using CS_APIServerProject.Services;
using CS_APIServerProject.Repository;



namespace CS_APIServerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IMapper _maper;
        private readonly DataBaseContext _db;
        private readonly IFileStorage _fs;
        private readonly IProductRepository _productRepository;
        private readonly ILogger _logger;

        public ProductController(DataBaseContext db, IMapper mapper, IFileStorage fs, IProductRepository productRepository, ILogger logger)
        {
            _maper = mapper;
            _db = db;
            _fs = fs;
            _productRepository = productRepository;
            _logger = logger;
        }



        //[HttpGet]
        //public async Task<ActionResult<List<ProductReadDTO>>> GetAll()
        //{
        //    var items = await _db.Products.ToListAsync();
        //    return Ok(_maper.Map<ProductReadDTO>(items));
        //}

        //Getting products with filtering, sorting and pagination
        [HttpGet("GetProducts")]
        public async Task<ActionResult<PageResult<ProductReadDTO>>> Get([FromBody] ProductQuery q)
        {
            //Filtering
            IQueryable<Product> query = _db.Products.
                AsNoTracking();
            if(!string.IsNullOrWhiteSpace(q.Brand))
            {
                _logger.LogInformation("Filtering by brand: {Brand}", q.Brand);
                query = query.Where(p => 
                p.Brand == q.Brand);
            }
            if(!string.IsNullOrWhiteSpace(q.State))
            {
                _logger.LogInformation("Filtering by state: {State}", q.State);
                query = query.Where(p => 
                p.Characteristics.state == q.State);
            }
            if (q.PriceFrom > 0)
            {
                _logger.LogInformation("Filtering by price from: {PriceFrom}", q.PriceFrom);
                query = query.Where(p => p.Price >= q.PriceFrom);
            }
            if (q.PriceTo > 0) {
                _logger.LogInformation("Filtering by price to: {PriceTo}", q.PriceTo);
                query = query.Where(p => p.Price <= q.PriceTo);
            }

            //Sorting
            bool desc = string.Equals(q.SortDir, "desc", StringComparison.OrdinalIgnoreCase);

            query = q.SortBy.ToLower() switch
            {
                "price" => desc ? query.OrderByDescending(o => o.Price) : query.OrderBy(o => o.Price),

                "quantity" => desc ? query.OrderByDescending(o => o.Quanity) : query.OrderBy(o => o.Quanity),

                "brand" => desc ? query.OrderByDescending(o => o.Brand) : query.OrderBy(o => o.Brand),

                _ => query.OrderBy(o => o.Brand)
                //_ => desc ? query.OrderByDescending(o => o.Brand) : query.OrderBy(o => o.Brand),

            };

            //Total count
            var totalCount = await query.CountAsync();
            //Pagination
            var items = await query.Skip((q.Page - 1) * q.PageSize)
                .Take(q.PageSize)
                .ToListAsync();

            var result = _maper.Map<List<ProductReadDTO>>(items);

            //Returning paginated result in format of new object.
            return Ok(new
            {
                totalCount,
                q.Page,
                q.PageSize,
                Data = result
            });
        }

        //Getting product by id
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductReadDTO>> GetById(Guid Id, CancellationToken ct )
        {
            var result = await _productRepository.GetProductById(Id, ct);
            if (result == null)
            {
                _logger.LogWarning("Product with id {Id} not found", Id);
                return NotFound();
            }
            return Ok(result);
            
        }

        //Creating product with image upload
        [HttpPost("create-product")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductCreateDTO>> CreateProduct([FromForm] ProductCreateDTO product
            , CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid product data received");
                return ValidationProblem(ModelState);
            }

            var result = await _productRepository.AddAsync(product, ct);

            //if (product.Image != null && product.Image.Length > 0)
            //{
            //    var imagePath = await _fs.SaveProductImageAsync(product.Image, ct);
            //    entity.ImagePath = imagePath;
            //}

            return Ok(result.Result); //CreatedAtAction(nameof(await _productRepository.GetProductById(result.Id), new { id = entity.Id }, result);
        }




        //That's how don't need to do.
        //[HttpPut("{id.guid}")]
        //public async Task<ActionResult<Product>> Update(Guid id , [FromBody] Product product)
        //{
        //    var dbproduct = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);

        //    if(dbproduct == null) { return NotFound(); }

        //    product.Characteristics ??= new Characteristics();
        //    dbproduct.Characteristics ??= new Characteristics();

        //    dbproduct.Brand = product.Brand;
        //    dbproduct.Model = product.Model;
        //    dbproduct.Description = product.Description;
        //    dbproduct.Price = product.Price;
        //    dbproduct.Quanity = product.Quanity;
        //    dbproduct.FK_Salesman = product.FK_Salesman;
        //    dbproduct.Currency = product.Currency;

        //    dbproduct.Characteristics.state = product.Characteristics.state;
        //    dbproduct.Characteristics.typeGas = product.Characteristics.typeGas;
        //    dbproduct.Characteristics.milege = product.Characteristics.milege;
        //    dbproduct.Characteristics.typeMilege = product.Characteristics.typeMilege;
        //    dbproduct.Characteristics.typeBody = product.Characteristics.typeBody;
        //    dbproduct.Characteristics.Color = product.Characteristics.Color;
        //    dbproduct.Characteristics.DriveType = product.Characteristics.DriveType;
        //    dbproduct.Characteristics.Engine = product.Characteristics.Engine;

        //    return Ok();
        //}

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDTO product , CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid product data received for update with id {Id}", id);
                return ValidationProblem(ModelState);
            }

            var entity = await _db.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity == null) { 
                _logger.LogWarning("Product with id {Id} not found for update", id);
                return NotFound(); }
            entity.Characteristics ??= new Characteristics();
            if(entity.Characteristics == null)
            {
                _logger.LogInformation("Initializing characteristics for product with id {Id} during update", id);
                entity.Characteristics = new Characteristics();
            }
            _maper.Map(product, entity);
            await _db.SaveChangesAsync(ct);
            return Ok();
        }

        //[HttpDelete]
        //public async Task<ActionResult<Product>> Delete(Guid id)
        //{
        //    var item = await _db.Products.FindAsync(id);

        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    _db.Products.Remove(item);

        //    return Ok();
        //}

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var entity = await _db.Products.FirstOrDefaultAsync(x => x.Id==id, ct);
            if (entity == null) { 
                _logger.LogWarning("Product with id {Id} not found for deletion", id);
                return NotFound(); }
            //if (entity.Image != null)
            //{
            //    await _db.Products.Remove(entity.Image);
            //}
            _db.Products.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return Ok();
        }



    }
}
