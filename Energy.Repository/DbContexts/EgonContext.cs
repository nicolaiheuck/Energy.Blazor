using Energy.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Energy.Repositories.DbContexts;

public class EgonContext : DbContext
{
    public EgonContext(DbContextOptions<EgonContext> options) : base(options)
    {
        
    }
    
    public DbSet<DataReading> DataReadings { get; set; }
    public DbSet<Location> Locations { get; set; }
}