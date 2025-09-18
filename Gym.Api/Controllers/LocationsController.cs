using Gym.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        public LocationsController(AppDbContext ctx) => _ctx = ctx;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _ctx.Locations.AsNoTracking().ToListAsync());

        // /api/locations/search?zip=10001 (exact), or prefix match like 100
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string zip)
        {
            if (string.IsNullOrWhiteSpace(zip)) return BadRequest("zip required");
            var z = zip.Trim();
            var results = await _ctx.Locations
                .Where(l => l.ZipCode == z || l.ZipCode.StartsWith(z))
                .AsNoTracking().ToListAsync();
            return Ok(results);
        }
    }
}
