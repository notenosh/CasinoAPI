namespace CasinoAPI.Models;

public class GameHistory
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string GameType { get; set; } = string.Empty;

    public decimal BetAmount { get; set; }

    public decimal WinAmount { get; set; }

    public DateTime PlayedAt { get; set; }
}