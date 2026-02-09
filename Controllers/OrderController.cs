using AutoMapper;
using CS_APIServerProject.Data;
using CS_APIServerProject.DTO;
using CS_APIServerProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CS_APIServerProject.Controllers
{
    public class OrderController : Controller
    {

        private readonly IMapper _maper;
        private readonly DBContext _db;

        public OrderController(DBContext db, IMapper mapper)
        {
            _maper = mapper;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get all Orders
        [HttpGet]
        public async Task<ActionResult<List<OrderReadDTO>>> GetAllOrders()
        {
            var items = await _db.Orders.ToListAsync();
            if (items == null) return NotFound();
            return Ok(items);
        }
        //Get one order by Id
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderReadDTO>> GetOrder(Guid id)
        {
            var item = await _db.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        //Create order
        [HttpPost("create-user")]
        public async Task<ActionResult<OrderCreateDTO>> CreateOrder([FromBody] UserCreateDTO orders)
        {
            if (!ModelState.IsValid) { return ValidationProblem(ModelState); }

            var entity = _maper.Map<Order>(orders);
            if (entity == null) return NotFound();
            entity.Id = Guid.NewGuid();
            await _db.Orders.AddAsync(entity);
            await _db.SaveChangesAsync();

            var result = _maper.Map<OrderReadDTO>(entity);
            return CreatedAtAction(nameof(GetOrder), new { id = entity.Id }, result);
        }

        //Update order
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UserUpdateDTO orders)
        {
            if (!ModelState.IsValid) { return ValidationProblem(ModelState); }

            var entity = await _db.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) { return NotFound(); }

            _maper.Map(orders, entity);
            await _db.SaveChangesAsync();
            return Ok();
        }

        //My try to write "removal" method to Order:
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            var entity = await _db.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) { return NotFound(); }
            _db.Orders.Remove(entity);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
