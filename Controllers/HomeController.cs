using CS_APIServerProject.Data;
using CS_APIServerProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CS_APIServerProject.DTO;
using AutoMapper;



namespace CS_APIServerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly IMapper _maper;
        private readonly DBContext _db;

        public HomeController(DBContext db, IMapper mapper)
        {
            _maper = mapper;
            _db = db;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductReadDTO>>> GetAll()
        {
            var items = await _db.Products.ToListAsync();
            return Ok(_maper.Map<ProductReadDTO>(items));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductReadDTO>> GetById(Guid Id)
        {
            var item = await _db.Products.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(_maper.Map<ProductReadDTO>(item));
        }


        [HttpPost("create-product")]
        public async Task<ActionResult<ProductCreateDTO>> CreateProduct([FromBody] ProductCreateDTO product )
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var entity = _maper.Map<Product>(product);

            entity.Id = Guid.NewGuid();
            entity.Characteristics ??= new Characteristics();
            _db.Products.Add(entity);
            await _db.SaveChangesAsync();

            var result = _maper.Map<ProductReadDTO>(entity);
            return CreatedAtAction(nameof(GetById), new {id = entity.Id}, result);
        }


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
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDTO product)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var entity = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) { return NotFound(); }
            entity.Characteristics ??= new Characteristics();

            _maper.Map(product, entity);
            await _db.SaveChangesAsync();
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
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _db.Products.FirstOrDefaultAsync(x => x.Id==id);
            if (entity == null) { return NotFound(); }
            _db.Products.Remove(entity);
            await _db.SaveChangesAsync();
            return Ok();
        }


        //CRUD for User

        //Get All Users
        [HttpGet]
        public async Task<ActionResult<List<UserReadDTO>>> GetAllUser()
        {
            var items = await _db.Users.ToListAsync();
            if(items == null) return NotFound();
            return Ok(items);
        }

        //Get one user by Id
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserReadDTO>> GetUser(Guid id)
        {
            var item = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost("create-user")]
        public async Task<ActionResult<UserCreateDTO>> CreateUser([FromBody] UserCreateDTO user)
        {
            if (!ModelState.IsValid){ return ValidationProblem(ModelState); }

            var entity = _maper.Map<User>(user);
            if (entity == null) return NotFound();
            entity.Id = Guid.NewGuid();
            await _db.Users.AddAsync(entity);
            await _db.SaveChangesAsync();

            var result = _maper.Map<UserReadDTO>(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateDTO user)
        {
            if (!ModelState.IsValid) { return ValidationProblem(ModelState); }

            var entity = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) { return NotFound(); }

            _maper.Map(user, entity);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var entity = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) { return NotFound(); }
            _db.Users.Remove(entity);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
