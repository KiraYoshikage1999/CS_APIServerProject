using CS_APIServerProject.Data;
using CS_APIServerProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CS_APIServerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly DBContext _db;

        public HomeController(DBContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Product>> GetById(Guid Id)
        {
            var item = await _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<Product>> GetProducts()
        {
            var items = await _db.Products.ToListAsync();

            if (items == null) { return NotFound(); }

            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product )
        {
            product.Id = Guid.NewGuid();
            product.Characteristics ??= new Characteristics();

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new {id = product.Id }, product);
        }

        [HttpPut("{id.guid}")]
        public async Task<ActionResult<Product>> Update(Guid id , [FromBody] Product product)
        {
            var dbproduct = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);

            if(dbproduct == null) { return NotFound(); }

            product.Characteristics ??= new Characteristics();
            dbproduct.Characteristics ??= new Characteristics();

            dbproduct.Brand = product.Brand;
            dbproduct.Model = product.Model;
            dbproduct.Description = product.Description;
            dbproduct.Price = product.Price;
            dbproduct.quanity = product.quanity;
            dbproduct.FK_Salesman = product.FK_Salesman;
            dbproduct.currency = product.currency;

            dbproduct.Characteristics.state = product.Characteristics.state;
            dbproduct.Characteristics.typeGas = product.Characteristics.typeGas;
            dbproduct.Characteristics.Milege = product.Characteristics.Milege;
            dbproduct.Characteristics.typeMilege = product.Characteristics.typeMilege;
            dbproduct.Characteristics.typeBody = product.Characteristics.typeBody;
            dbproduct.Characteristics.Color = product.Characteristics.Color;
            dbproduct.Characteristics.DriveType = product.Characteristics.DriveType;
            dbproduct.Characteristics.Engine = product.Characteristics.Engine;

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<Product>> Delete(Guid id)
        {
            var item = await _db.Products.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _db.Products.Remove(item);

            return Ok();
        }
     }
}
