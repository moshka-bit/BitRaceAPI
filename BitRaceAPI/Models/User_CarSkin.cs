using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitRaceAPI.Models;

public class User_CarSkin
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
    
    [ForeignKey("CarSkin")]
    public int CarSkinId { get; set; }
    public CarSkin CarSkin { get; set; }
}