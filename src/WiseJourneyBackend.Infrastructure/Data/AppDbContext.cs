using Microsoft.EntityFrameworkCore;
using WiseJourneyBackend.Domain.Entities.Auth;
using WiseJourneyBackend.Domain.Entities.Places;
using WiseJourneyBackend.Domain.Entities.Trips;
namespace WiseJourneyBackend.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Geometry> Geometries { get; set; }
    public DbSet<OpeningHour> OpeningHours { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<PlaceType> PlacesType { get; set; }
    public DbSet<WeeklyHour> WeeklyHours { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<TripDay> TripDays { get; set; }
    public DbSet<TripPlace> TripPlaces { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}