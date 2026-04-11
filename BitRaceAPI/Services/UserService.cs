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
        
        var theSameEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == registringUser.Email.ToLower());

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
            Password =  registringUser.Password,
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        return new OkObjectResult(new
        {
            status = true
        });
    }

    public async Task<IActionResult> AuthUserAsync(authUser authUser)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == authUser.Email && u.Password == authUser.Password);
        
        if (user == null)
        {
            return new NotFoundObjectResult(new
            {
                status = false,
                message = "Нет такого пользователя"
            });
        };
        
        return new OkObjectResult(new
        {
            userId = user.Id,
            status = true
        });
    }
}