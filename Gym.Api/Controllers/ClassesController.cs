using Gym.Api.Data;
using Gym.Api.DTOs;
using Gym.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Gym.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        public ClassesController(AppDbContext ctx) => _ctx = ctx;

        // List upcoming classes (optional filter by location)
        // List upcoming classes with booked flag
// List upcoming classes (optionally filter by location or "mine")
[HttpGet]
public async Task<IActionResult> List([FromQuery] int? locationId, [FromQuery] bool? mine)
{
    var q = _ctx.Classes
        .Include(c => c.Location)
        .Include(c => c.Bookings)
        .AsQueryable();

    if (locationId.HasValue)
        q = q.Where(c => c.LocationId == locationId.Value);

    // Who is calling? (token optional)
    var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
    int? userId = int.TryParse(userIdStr, out var uid) ? uid : null;

    // If mine=true and we have a user, filter to bookings for that user
    if (mine == true && userId is int u)
        q = q.Where(c => c.Bookings.Any(b => b.UserId == u));

    var upcoming = await q.Where(c => c.StartUtc >= DateTime.UtcNow)
        .OrderBy(c => c.StartUtc)
        .Take(200)
        .AsNoTracking()
        .Select(c => new Gym.Api.DTOs.ClassItemDto(
            c.Id,
            c.Title,
            c.Description,
            c.Capacity,
            c.StartUtc,
            c.EndUtc,
            c.LocationId,
            c.Location.Name,
            userId != null && c.Bookings.Any(b => b.UserId == userId),
            c.Capacity - c.Bookings.Count
        ))
        .ToListAsync();

    return Ok(upcoming);
}


// My booked classes
[Authorize]
[HttpGet("mine")]
public async Task<IActionResult> Mine()
{
    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
    var mine = await _ctx.Bookings
        .Where(b => b.UserId == userId)
        .Select(b => b.ClassId)
        .ToListAsync();

    var items = await _ctx.Classes
        .Include(c => c.Location)
        .Include(c => c.Bookings)
        .Where(c => mine.Contains(c.Id))
        .OrderBy(c => c.StartUtc)
        .AsNoTracking()
        .Select(c => new ClassItemDto(
            c.Id, c.Title, c.Description, c.Capacity,
            c.StartUtc, c.EndUtc, c.LocationId, c.Location.Name,
            true, c.Capacity - c.Bookings.Count))
        .ToListAsync();

    return Ok(items);
}

        // Book a class (member)
        [Authorize]
        [HttpPost("book")]
        public async Task<IActionResult> Book(BookClassRequest req)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var cls = await _ctx.Classes.Include(c => c.Bookings)
                                        .FirstOrDefaultAsync(c => c.Id == req.ClassId);
            if (cls == null) return NotFound("Class not found");
            if (cls.Bookings.Count >= cls.Capacity) return BadRequest("Class is full");

            var exists = await _ctx.Bookings.AnyAsync(b => b.ClassId == req.ClassId && b.UserId == userId);
            if (exists) return BadRequest("Already booked");

            _ctx.Bookings.Add(new ClassBooking { ClassId = req.ClassId, UserId = userId });
            await _ctx.SaveChangesAsync();
            return Ok(new { message = "Booked" });
        }

        // Cancel booking (member)
        [Authorize]
        [HttpDelete("book/{classId:int}")]
        public async Task<IActionResult> Cancel(int classId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var booking = await _ctx.Bookings.FirstOrDefaultAsync(b => b.ClassId == classId && b.UserId == userId);
            if (booking == null) return NotFound();

            _ctx.Bookings.Remove(booking);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }

        // ---------- Admin CRUD for classes ----------
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(FitnessClass input)
        {
            _ctx.Classes.Add(input);
            await _ctx.SaveChangesAsync();
            return Created($"/api/classes/{input.Id}", input);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, FitnessClass input)
        {
            var cls = await _ctx.Classes.FindAsync(id);
            if (cls == null) return NotFound();

            cls.Title = input.Title;
            cls.Description = input.Description;
            cls.Capacity = input.Capacity;
            cls.StartUtc = input.StartUtc;
            cls.EndUtc = input.EndUtc;
            cls.LocationId = input.LocationId;

            await _ctx.SaveChangesAsync();
            return Ok(cls);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cls = await _ctx.Classes.FindAsync(id);
            if (cls == null) return NotFound();

            _ctx.Classes.Remove(cls);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
