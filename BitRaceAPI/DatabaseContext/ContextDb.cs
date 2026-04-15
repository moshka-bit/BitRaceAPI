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
    public DbSet<Level> Levels { get; set; }
    public DbSet<User_Level>  User_Levels { get; set; }
    public DbSet<CarSkin> CarSkins { get; set; }
    public DbSet<User_CarSkin>  User_CarSkins { get; set; }
}