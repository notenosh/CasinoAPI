using Microsoft.AspNetCore.Mvc;
using CasinoAPI.Data;
using CasinoAPI.Models;
using CasinoAPI.DTOs;
using CasinoAPI.Services;
using System.Linq;

namespace CasinoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
private readonly CasinoDbContext _context;
private readonly JwtService _jwtService;

public AuthController(
    CasinoDbContext context,
    JwtService jwtService)
{
    _context = context;
    _jwtService = jwtService;
}

[HttpPost("register")]
public IActionResult Register(RegisterDto dto)
{
    var user = new User
    {
        Username = dto.Username,
        Email = dto.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        Balance = 1000
    };

    _context.Users.Add(user);
    _context.SaveChanges();

    return Ok("User Registered");
}

[HttpPost("login")]
public IActionResult Login(LoginDto dto)
{
    var user = _context.Users
        .FirstOrDefault(u => u.Email == dto.Email);

    if (user == null)
        return Unauthorized("Invalid Email");

    bool validPassword =
        BCrypt.Net.BCrypt.Verify(
            dto.Password,
            user.PasswordHash);

    if (!validPassword)
        return Unauthorized("Invalid Password");

    var token = _jwtService.GenerateToken(user);

    return Ok(new
    {
        Message = "Login Successful",
        Token = token,
        User = user.Username,
        Balance = user.Balance
    });
}

[HttpGet("balance/{email}")]
public IActionResult GetBalance(string email)
{
    var user = _context.Users
        .FirstOrDefault(u => u.Email == email);

    if (user == null)
        return NotFound("User not found");

    return Ok(new
    {
        User = user.Username,
        Balance = user.Balance
    });
}

[HttpPost("deposit")]
public IActionResult Deposit(TransactionDto dto)
{
    var user = _context.Users
        .FirstOrDefault(u => u.Email == dto.Email);

    if (user == null)
        return NotFound("User not found");

    user.Balance += dto.Amount;

    _context.SaveChanges();

    return Ok(new
    {
        Message = "Deposit Successful",
        Balance = user.Balance
    });
}

[HttpPost("withdraw")]
public IActionResult Withdraw(TransactionDto dto)
{
    var user = _context.Users
        .FirstOrDefault(u => u.Email == dto.Email);

    if (user == null)
        return NotFound("User not found");

    if (user.Balance < dto.Amount)
        return BadRequest("Insufficient Balance");

    user.Balance -= dto.Amount;

    _context.SaveChanges();

    return Ok(new
    {
        Message = "Withdrawal Successful",
        Balance = user.Balance
    });
}
}
