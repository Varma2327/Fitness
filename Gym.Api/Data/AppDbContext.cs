using Microsoft.EntityFrameworkCore;
using Gym.Api.Models;


namespace Gym.Api.Data
{
public class AppDbContext : DbContext
{
public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}


public DbSet<User> Users => Set<User>();
public DbSet<GymLocation> Locations => Set<GymLocation>();
public DbSet<FitnessClass> Classes => Set<FitnessClass>();
public DbSet<ClassBooking> Bookings => Set<ClassBooking>();
public DbSet<CheckIn> CheckIns => Set<CheckIn>();
public DbSet<WorkoutLog> WorkoutLogs => Set<WorkoutLog>();


protected override void OnModelCreating(ModelBuilder b)
{
b.Entity<User>()
.HasIndex(u => u.Email)
.IsUnique();


b.Entity<ClassBooking>()
.HasIndex(bk => new { bk.ClassId, bk.UserId })
.IsUnique();


b.Entity<FitnessClass>()
.HasOne(c => c.Location)
.WithMany(l => l.Classes)
.HasForeignKey(c => c.LocationId);


b.Entity<ClassBooking>()
.HasOne(bk => bk.Class)
.WithMany(c => c.Bookings)
.HasForeignKey(bk => bk.ClassId);


b.Entity<ClassBooking>()
.HasOne(bk => bk.User)
.WithMany(u => u.Bookings)
.HasForeignKey(bk => bk.UserId);
}
}
}