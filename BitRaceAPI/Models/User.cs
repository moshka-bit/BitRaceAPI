using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitRaceAPI.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public int Money { get; set; }
    
    [ForeignKey("CarSkin")]
    
    public int? CarSkinId { get; set; }
    public CarSkin? CarSkin { get; set; }
}