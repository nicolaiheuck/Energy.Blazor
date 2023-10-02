using Energy.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Energy.Repositories.DbContexts;

public class EgonContext : DbContext
{
    public EgonContext(DbContextOptions<EgonContext> options) : base(options)
    {
        
    }
    
    public DbSet<Telemetry> Telemetry { get; set; }
    public DbSet<Location> Locations { get; set; }
}