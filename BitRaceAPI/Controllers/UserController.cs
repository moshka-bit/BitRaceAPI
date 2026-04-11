using BitRaceAPI.Requests;
using BitRaceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BitRaceAPI.Controllers;

public class UserController
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    [Route("RegistrationNewUser")]
    public async Task<IActionResult> RegistrationNewUser([FromBody]RegistringUser registringUser)
    {
        return await _userService.RegistrationNewUserAsync(registringUser);
    }    
    [HttpPost]
    [Route("authUser")]
    public async Task<IActionResult> AuthUser([FromBody]authUser authUser)
    {
        return await _userService.AuthUserAsync(authUser);
    }
}