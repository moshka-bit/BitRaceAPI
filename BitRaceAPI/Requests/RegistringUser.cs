using System.Text.Json.Serialization;

namespace BitRaceAPI.Requests;

public class RegistringUser
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}