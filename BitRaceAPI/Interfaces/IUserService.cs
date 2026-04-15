using BitRaceAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BitRaceAPI.Interfaces;

public interface IUserService
{
    // регистрация
    Task<IActionResult> RegistrationNewUserAsync(RegistringUser registringUser);
    // авторизация
    Task<IActionResult> AuthUserAsync(authUser authUser);
    // получение рекордов по уровням
    Task<IActionResult> GetScoresByLevels(int userId);
    // обновление рекорда пользователя
    Task<IActionResult> PutScore(int userId, int levelId, int score);
    // получение количества монет пользователя
    Task<IActionResult> GetMoneyByUserId(int userId);
    // добавление монет пользователю
    Task<IActionResult> AddMoneyToUser(int userId, int money);
    // получение всех купленных скинов и экипированного скина
    Task<IActionResult> GetAllSkinsAndEquippedByUserId(int userId);
    // покупка скина
    Task<IActionResult> BuySkin(int userId, int skinId);
    // экипировка скина
    Task<IActionResult> EquipSkin(int userId, int skinId);
    // топ 10 рекордов
    Task<IActionResult> GetTop10Records();
}