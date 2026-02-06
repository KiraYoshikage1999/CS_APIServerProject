using AutoMapper;
using CS_APIServerProject.DTO;
using CS_APIServerProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CS_APIServerProject.Data;

namespace CS_APIServerProject.Controllers
{
    public class UserController : Controller
    {

        private readonly IMapper _maper;
        private readonly DBContext _db;

        public UserController(DBContext db, IMapper mapper)
        {
            _maper = mapper;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }




        //CRUD for User

        //Get All Users
        [HttpGet]
        public async Task<ActionResult<List<UserReadDTO>>> GetAllUser()
        {
            var items = await _db.Users.ToListAsync();
            if (items == null) return NotFound();
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
            if (!ModelState.IsValid) { return ValidationProblem(ModelState); }

            var entity = _maper.Map<User>(user);
            if (entity == null) return NotFound();
            entity.Id = Guid.NewGuid();
            await _db.Users.AddAsync(entity);
            await _db.SaveChangesAsync();

            var result = _maper.Map<UserReadDTO>(entity);
            return CreatedAtAction(nameof(GetUser), new { id = entity.Id }, result);
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
