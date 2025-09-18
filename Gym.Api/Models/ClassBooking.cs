namespace Gym.Api.Models
{
public class ClassBooking
{
public int Id { get; set; }
public int ClassId { get; set; }
public FitnessClass? Class { get; set; }
public int UserId { get; set; }
public User? User { get; set; }
public DateTime BookedAtUtc { get; set; } = DateTime.UtcNow;
public string Status { get; set; } = "booked"; // booked|cancelled|attended|no_show
}
}