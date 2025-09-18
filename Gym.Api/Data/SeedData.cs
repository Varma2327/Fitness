using Gym.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Gym.Api.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(AppDbContext ctx)
        {
            if (!await ctx.Locations.AnyAsync())
            {
                ctx.Locations.AddRange(
                    new GymLocation { Name = "NYC – Midtown", Address = "123 7th Ave, New York, NY", ZipCode = "10001", Timezone = "America/New_York" },
                    new GymLocation { Name = "LA – Culver City", Address = "111 Washington Blvd, Culver City, CA", ZipCode = "90232", Timezone = "America/Los_Angeles" }
                );
                await ctx.SaveChangesAsync();
            }

            // add two FUTURE classes if none upcoming
            var hasUpcoming = await ctx.Classes.AnyAsync(c => c.StartUtc >= DateTime.UtcNow);
            if (!hasUpcoming)
            {
                var firstLoc = await ctx.Locations.FirstAsync();
                var now = DateTime.UtcNow;

                ctx.Classes.AddRange(
                    new FitnessClass {
                        Title = "HIIT Express",
                        Description = "30-min HIIT",
                        Capacity = 15,
                        StartUtc = now.AddHours(24),
                        EndUtc   = now.AddHours(24).AddMinutes(30),
                        LocationId = firstLoc.Id
                    },
                    new FitnessClass {
                        Title = "Yoga Flow",
                        Description = "Mindful vinyasa",
                        Capacity = 20,
                        StartUtc = now.AddHours(26),
                        EndUtc   = now.AddHours(27),
                        LocationId = firstLoc.Id
                    }
                );
                await ctx.SaveChangesAsync();
                Console.WriteLine("[Seed] Inserted demo classes.");
            }
        }
    }
}
