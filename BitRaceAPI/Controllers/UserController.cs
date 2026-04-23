using BitRaceAPI.Requests;
using BitRaceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BitRaceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController: ControllerBase
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
    [HttpGet]
    [Route("GetScoresByLevels")]
    public async Task<IActionResult> GetScoresByLevels([FromQuery]int userId)
    {
        return await _userService.GetScoresByLevels(userId);
    }
    
    [HttpPut]
    [Route("PutScore")]
    public async Task<IActionResult> PutScore([FromQuery]int userId, [FromQuery]int levelId, [FromQuery]int score)
    {
        return await _userService.PutScore(userId,  levelId, score);
    }
    
    [HttpGet]
    [Route("GetMoneyByUserId")]
    public async Task<IActionResult> GetMoneyByUserId([FromQuery] int userId)
    {
        return await _userService.GetMoneyByUserId(userId);
    }

    [HttpPost]
    [Route("AddMoneyToUser")]
    public async Task<IActionResult> AddMoneyToUser([FromQuery] int userId, [FromQuery] int money)
    {
        return await _userService.AddMoneyToUser(userId, money);
    }

    [HttpGet]
    [Route("GetAllSkinsAndEquippedByUserId")]
    public async Task<IActionResult> GetAllSkinsAndEquippedByUserId([FromQuery] int userId)
    {
        return await _userService.GetAllSkinsAndEquippedByUserId(userId);
    }

    [HttpPost]
    [Route("BuySkin")]
    public async Task<IActionResult> BuySkin([FromQuery] int userId, [FromQuery] int skinId)
    {
        return await _userService.BuySkin(userId, skinId);
    }

    [HttpPost]
    [Route("EquipSkin")]
    public async Task<IActionResult> EquipSkin([FromQuery] int userId, [FromQuery] int skinId)
    {
        return await _userService.EquipSkin(userId, skinId);
    }
    
    [HttpGet]
    [Route("GetTop10Records")]
    public async Task<IActionResult> GetTop10Records()
    {
        return await _userService.GetTop10Records();
    }
}