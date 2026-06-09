using Microsoft.AspNetCore.Mvc;
using CasinoAPI.Data;
using CasinoAPI.DTOs;
using CasinoAPI.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CasinoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly CasinoDbContext _context;

    public GameController(CasinoDbContext context)
    {
        _context = context;
    }

    [HttpPost("slot")]
    public IActionResult PlaySlot(GameBetDto dto)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Email == dto.Email);

        if (user == null)
            return NotFound("User not found");

        if (user.Balance < dto.BetAmount)
            return BadRequest("Insufficient Balance");

        string[] symbols = { "🍒", "🍋", "⭐", "7️⃣" };

        Random random = new();

        string s1 = symbols[random.Next(symbols.Length)];
        string s2 = symbols[random.Next(symbols.Length)];
        string s3 = symbols[random.Next(symbols.Length)];

        decimal winnings = 0;

        if (s1 == s2 && s2 == s3)
            winnings = dto.BetAmount * 5;
        else if (s1 == s2 || s2 == s3 || s1 == s3)
            winnings = dto.BetAmount * 2;
        else
            winnings = -dto.BetAmount;

        user.Balance += winnings;

        var history = new GameHistory
        {
            Email = dto.Email,
            GameType = "Slot Machine",
            BetAmount = dto.BetAmount,
            WinAmount = winnings,
            PlayedAt = DateTime.UtcNow
        };

        _context.GameHistories.Add(history);
        _context.SaveChanges();

        return Ok(new
        {
            Reels = $"{s1} {s2} {s3}",
            Result = winnings >= 0 ? "Win" : "Lose",
            Winnings = winnings,
            NewBalance = user.Balance
        });
    }

    [HttpGet("history/{email}")]
    public IActionResult GetHistory(string email)
    {
        var history = _context.GameHistories
            .Where(h => h.Email == email)
            .OrderByDescending(h => h.PlayedAt)
            .ToList();

        return Ok(history);
    }
}