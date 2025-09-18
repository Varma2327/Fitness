namespace Gym.Api.Models
{
public class CheckIn
{
public int Id { get; set; }
public int UserId { get; set; }
public int LocationId { get; set; }
public DateTime CheckedInUtc { get; set; } = DateTime.UtcNow;
}
}