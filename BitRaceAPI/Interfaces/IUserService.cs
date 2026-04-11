using BitRaceAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BitRaceAPI.Interfaces;

public interface IUserService
{
    Task<IActionResult> RegistrationNewUserAsync(RegistringUser registringUser);
    Task<IActionResult> AuthUserAsync(authUser authUser);
}