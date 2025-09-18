using Gym.Api.Data;
using Gym.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Gym.Api.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class CheckInsController : ControllerBase
{
private readonly AppDbContext _ctx;
public CheckInsController(AppDbContext ctx) => _ctx = ctx;


[Authorize]
[HttpPost]
public async Task<IActionResult> Create([FromQuery] int locationId)
{
var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
var locExists = await _ctx.Locations.AnyAsync(l => l.Id == locationId);
if (!locExists) return NotFound("Location not found");


var ci = new CheckIn { UserId = userId, LocationId = locationId };
_ctx.CheckIns.Add(ci);
await _ctx.SaveChangesAsync();
return Ok(ci);
}
}
}