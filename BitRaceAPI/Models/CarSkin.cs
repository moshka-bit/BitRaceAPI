using System.ComponentModel.DataAnnotations;

namespace BitRaceAPI.Models;

public class CarSkin
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
}