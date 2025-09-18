namespace Gym.Api.Models
{
public class FitnessClass
{
public int Id { get; set; }
public string Title { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public int Capacity { get; set; }
public DateTime StartUtc { get; set; }
public DateTime EndUtc { get; set; }


public int LocationId { get; set; }
public GymLocation? Location { get; set; }


public ICollection<ClassBooking> Bookings { get; set; } = new List<ClassBooking>();
}
}