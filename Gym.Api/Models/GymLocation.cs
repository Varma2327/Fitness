namespace Gym.Api.Models
{
    public class GymLocation
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ZipCode { get; set; } = "";     // <-- add this
        public string Timezone { get; set; } = "America/New_York";

        public ICollection<FitnessClass> Classes { get; set; } = new List<FitnessClass>();
    }
}
