namespace CasinoAPI.DTOs;

public class GameBetDto
{
    public string Email { get; set; } = string.Empty;

    public decimal BetAmount { get; set; }
}