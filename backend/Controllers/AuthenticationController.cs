using backend.Data;
using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using backend.Libs;
using backend.Models;

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
}