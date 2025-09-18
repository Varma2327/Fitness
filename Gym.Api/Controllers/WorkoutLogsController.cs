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
    [Authorize]
    public class WorkoutLogsController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        public WorkoutLogsController(AppDbContext ctx) => _ctx = ctx;

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var userId = CurrentUserId;
            var logs = await _ctx.WorkoutLogs
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.DateUtc)
                .AsNoTracking().ToListAsync();
            return Ok(logs);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkoutLog log)
        {
            log.UserId = CurrentUserId;
            log.DateUtc = log.DateUtc == default ? DateTime.UtcNow.Date : log.DateUtc;
            _ctx.WorkoutLogs.Add(log);
            await _ctx.SaveChangesAsync();
            return Created($"/api/workoutlogs/{log.Id}", log);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, WorkoutLog input)
        {
            var userId = CurrentUserId;
            var log = await _ctx.WorkoutLogs.FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
            if (log == null) return NotFound();

            log.Type = input.Type;
            log.Notes = input.Notes;
            log.DurationMinutes = input.DurationMinutes;
            log.Calories = input.Calories;
            await _ctx.SaveChangesAsync();
            return Ok(log);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = CurrentUserId;
            var log = await _ctx.WorkoutLogs.FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
            if (log == null) return NotFound();

            _ctx.WorkoutLogs.Remove(log);
            await _ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
