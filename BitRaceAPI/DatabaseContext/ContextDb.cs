using BitRaceAPI.Models;
using BitRaceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BitRaceAPI.DatabaseContext;

public class ContextDb: DbContext
{
    public ContextDb(DbContextOptions<ContextDb> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
}