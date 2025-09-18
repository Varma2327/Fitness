namespace Gym.Api.Models
{
public class WorkoutLog
{
public int Id { get; set; }
public int UserId { get; set; }
public DateTime DateUtc { get; set; } = DateTime.UtcNow.Date;
public string Type { get; set; } = string.Empty; // e.g., cardio, strength
public string Notes { get; set; } = string.Empty;
public int? DurationMinutes { get; set; }
public int? Calories { get; set; }
}
}