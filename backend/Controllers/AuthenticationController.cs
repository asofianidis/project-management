using backend.Data;
using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using backend.Libs;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("/api/[Controller]/[Action]")]
public class AuthenticationController : Controller
{
    private readonly AppDbContext _context;

    public AuthenticationController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegistrationDTO body)
    {
        try
        {
            var timeNow = new DateTime();
            timeNow = DateTime.Now;
            var salt = $"{Hash.ComputeSHA256Hash(timeNow.ToString())}";
            var newUser = new User();
            newUser.Id = 1;
            newUser.Email = body.Email;
            newUser.Created = DateTime.Now.ToUniversalTime();
            newUser.Salt = salt;
            newUser.Pass = $"{Hash.ComputeSHA256Hash(salt + body.Password)}";
            newUser.FirstName = body.FirstName;
            newUser.LastName = body.LastName;
            newUser.IsAdmin = false;
            
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(newUser));
            
            if (ModelState.IsValid)
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();   
            }
            else
            {
                return Ok(false);
            }
            

            return Ok(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDTO body)
    {
        try
        {
            var salt = await _context.Users.Where(user => user.Email == body.Username).Select(user => user.Salt).FirstAsync();
            var user = await _context.Users.Where(user1 => user1.Pass == Hash.ComputeSHA256Hash(salt + body.Password)).FirstOrDefaultAsync();
            if (user == null)
            {
                return StatusCode(204);
            }

            var rand = new Random();
            var token = Hash.ComputeSHA256Hash(DateTime.Now.ToString() + rand.Next(1000, 9999));
            var userToken = new UserToken();
            userToken.UserId = user.Id;
            userToken.Token = token;
            userToken.Expires = DateTime.Now.AddDays(90).ToUniversalTime();

            if (ModelState.IsValid)
            {
                var existingToken = _context.UserTokens.FirstOrDefault(userToken => userToken.UserId == user.Id);

                if (existingToken == null)
                {
                    await _context.UserTokens.AddAsync(userToken);
                }
                else
                {
                    existingToken.Token = token;
                    _context.UserTokens.Update(existingToken);
                }

                await _context.SaveChangesAsync();
            }

            
            
            
            return Ok(userToken.Token);

            Console.WriteLine(salt);
            return Ok("hello world");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500);
        }
    }
}