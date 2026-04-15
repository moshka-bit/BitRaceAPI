using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitRaceAPI.Models;

public class User_Level
{
    [Key]
    public int Id { get; set; }
    public int Record { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
    
    [ForeignKey("Level")]
    public int LevelId { get; set; }
    public Level Level { get; set; }
}