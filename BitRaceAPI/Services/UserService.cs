using BitRaceAPI.Requests;
using BitRaceAPI.DatabaseContext;
using BitRaceAPI.Interfaces;
using BitRaceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BitRaceAPI.Services;

public class UserService : IUserService
{
    private readonly ContextDb _context;

    public UserService(ContextDb context)
    {
        _context = context;
    }

    public async Task<IActionResult> RegistrationNewUserAsync(RegistringUser registringUser)
    {
        if (string.IsNullOrEmpty(registringUser.Name))
        {
            return new BadRequestObjectResult(new
            {
                status = false,
                message = "Имя не может быть пустым"
            });
        }

        if (string.IsNullOrEmpty(registringUser.Email))
        {
            return new BadRequestObjectResult(new
            {
                status = false,
                message = "Почта не может быть пустая"
            });
        }

        if (string.IsNullOrEmpty(registringUser.Password))
        {
            return new BadRequestObjectResult(new
            {
                status = false,
                message = "Пароль не может быть пустым"
            });
        }

        var theSameEmail =
            await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == registringUser.Email.ToLower());

        if (theSameEmail != null)
        {
            return new BadRequestObjectResult(new
            {
                status = false,
                message = "Пользователь с таким login уже существует"
            });
        }

        var user = new User()
        {
            Name = registringUser.Name,
            Email = registringUser.Email,
            Password = registringUser.Password,
            Money = 0,
            CarSkinId = 1 // автоматически экипирован первый скин
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var userCarSkin = new User_CarSkin()
        {
            UserId = user.Id,
            CarSkinId = 1 // даём стандартный скин
        };

        await _context.User_CarSkins.AddAsync(userCarSkin);
        await _context.SaveChangesAsync();

        return new OkObjectResult(new
        {
            status = true
        });
    }

    public async Task<IActionResult> AuthUserAsync(authUser authUser)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == authUser.Email && u.Password == authUser.Password);

        if (user == null)
        {
            return new NotFoundObjectResult(new
            {
                status = false,
                message = "Нет такого пользователя"
            });
        }

        ;

        return new OkObjectResult(new
        {
            userId = user.Id,
            status = true
        });
    }

    public async Task<IActionResult> GetScoresByLevels(int userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return new NotFoundObjectResult(new
            {
                status = false,
                message = "Нет такого пользователя с таким id"
            });
        }

        var data = await _context.User_Levels
            .Where(d => d.UserId == userId)
            .Select(d => new { d.LevelId, d.Record })
            .ToListAsync();

        return new OkObjectResult(new
        {
            status = true,
            data = data
        });
    }

    public async Task<IActionResult> PutScore(int userId, int levelId, int score)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return new NotFoundObjectResult(new
            {
                status = false,
                message = "Нет такого пользователя с таким id"
            });
        }

        var level = await _context.Levels.FirstOrDefaultAsync(u => u.Id == levelId);

        if (level == null)
        {
            return new NotFoundObjectResult(new
            {
                status = false,
                message = "Нет такого уровня с таким id"
            });
        }

        var userLevel =
            await _context.User_Levels.FirstOrDefaultAsync(u_l => u_l.UserId == userId && u_l.LevelId == levelId);

        userLevel.Record = score;

        await _context.SaveChangesAsync();

        return new OkObjectResult(new
        {
            status = true
        });
    }

    public async Task<IActionResult> GetMoneyByUserId(int userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return new NotFoundObjectResult(new
            {
                status = false,
                message = "Нет такого пользователя с таким id"
            });
        }

        return new OkObjectResult(new
        {
            status = true,
            money = user.Money
        });
    }

    public async Task<IActionResult> AddMoneyToUser(int userId, int money)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return new NotFoundObjectResult(new
            {
                status = false,
                message = "Нет такого пользователя с таким id"
            });
        }

        if (money < 0)
        {
            return new BadRequestObjectResult(new
            {
                status = false,
                message = "Сумма не может быть отрицательной"
            });
        }

        user.Money += money;

        await _context.SaveChangesAsync();

        return new OkObjectResult(new
        {
            status = true
        });
    }

    public async Task<IActionResult> GetAllSkinsAndEquippedByUserId(int userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return new NotFoundObjectResult(new
            {
                status = false,
                message = "Нет такого пользователя с таким id"
            });
        }

        var usersSkins = await _context.User_CarSkins.Where(uB => uB.UserId == user.Id).Select(uB => uB.CarSkinId)
            .ToListAsync();

        return new OkObjectResult(new
        {
            status = true,
            data = new { skins = usersSkins },
            equippedSkin = user.CarSkinId
        });
    }

    public async Task<IActionResult> BuySkin(int userId, int skinId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return new NotFoundObjectResult(new
            {
                status = false,
                message = "Нет такого пользователя с таким id"
            });
        }

        var skin = await _context.CarSkins.FirstOrDefaultAsync(s => s.Id == skinId);

        if (skin == null)
        {
            return new NotFoundObjectResult(new
            {
                status = false,
                message = "Нет такого скина с таким id"
            });
        }

        if (user.Money < skin.Price)
        {
            return new BadRequestObjectResult(new
            {
                status = false,
                message = "Недостаточно денежных средств для покупки скина"
            });
        }

        var boughtSkin =
            await _context.User_CarSkins.FirstOrDefaultAsync(u => u.UserId == user.Id && u.CarSkinId == skin.Id);

        if (boughtSkin != null)
        {
            return new BadRequestObjectResult(new
            {
                status = false,
                message = "Этот скин уже куплен"
            });
        }

        user.Money -= skin.Price;

        var userBallSkin = new User_CarSkin()
        {
            UserId = user.Id,
            CarSkinId = skin.Id
        };

        await _context.User_CarSkins.AddAsync(userBallSkin);
        await _context.SaveChangesAsync();

        return new OkObjectResult(new
        {
            status = true
        });
    }

    public async Task<IActionResult> EquipSkin(int userId, int skinId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return new NotFoundObjectResult(new
            {
                status = false,
                message = "Нет такого пользователя с таким id"
            });
        }

        var skin = await _context.CarSkins.FirstOrDefaultAsync(s => s.Id == skinId);
        if (skin == null)
        {
            return new NotFoundObjectResult(new
            {
                status = false,
                message = "Нет такого скина с таким id"
            });
        }

        user.CarSkinId = skinId;

        await _context.SaveChangesAsync();

        return new OkObjectResult(new
        {
            status = true
        });
    }

    public async Task<IActionResult> GetTop10Records()
    {
        var topUsers = await _context.User_Levels
            .Include(ul => ul.User)
            .GroupBy(ul => ul.UserId)
            .Select(g => new
            {
                userName = g.First().User.Name,
                totalScore = g.Sum(ul => ul.Record)
            })
            .OrderByDescending(u => u.totalScore)
            .Take(10)
            .ToListAsync();

        return new OkObjectResult(new
        {
            status = true,
            data = topUsers
        });
    }
}