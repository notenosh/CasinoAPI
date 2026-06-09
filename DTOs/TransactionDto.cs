namespace CasinoAPI.DTOs;

public class TransactionDto
{
    public string Email { get; set; } = string.Empty;

    public decimal Amount { get; set; }
}