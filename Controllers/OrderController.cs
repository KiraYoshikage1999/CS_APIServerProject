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
        public async Task<ActionResult<PageResult<OrderReadDTO>>> Get([FromBody] OrderQuery q)
        {
            //Filtering
            IQueryable<Order> query = _db.Orders.
                AsNoTracking();
            if (q.Number > 0)
            {
                query = query.Where(p =>
                p.Number == q.Number);
            }
            if (!string.IsNullOrWhiteSpace(q.Status))
            {
                query = query.Where(p =>
                p.Status == q.Status);
            }
            if (q.CreatedAt != null)
            {
                query = query.Where(p => 
                p.CreatedAt == q.CreatedAt);  
            }
            //if (q.PriceFrom > 0)
            //{
            //    query = query.Where(p => p.Price >= q.PriceTo);
            //}
            //if (q.PriceTo > 0)
            //{
            //    query = query.Where(p => p.Price <= q.PriceTo);
            //}

            //Sorting
            bool desc = string.Equals(q.SortDir, "desc", StringComparison.OrdinalIgnoreCase);

            query = q.SortBy.ToLower() switch
            {
                "createdAt" => desc ? query.OrderByDescending(o => o.CreatedAt) : query.OrderBy(o => o.CreatedAt),

                "status" => desc ? query.OrderByDescending(o => o.Status) : query.OrderBy(o => o.Status),

                "number" => desc ? query.OrderByDescending(o => o.Number) : query.OrderBy(o => o.Number),

                _ => query.OrderBy(o => o.Number)
                //_ => desc ? query.OrderByDescending(o => o.Brand) : query.OrderBy(o => o.Brand),

            };

            //Total count
            var totalCount = await query.CountAsync();
            //Pagination
            var items = await query.Skip((q.Page - 1) * q.PageSize)
                .Take(q.PageSize)
                .ToListAsync();

            var result = _maper.Map<List<OrderReadDTO>>(items);

            return Ok(new
            {
                totalCount,
                q.Page,
                q.PageSize,
                Data = result
            });
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
