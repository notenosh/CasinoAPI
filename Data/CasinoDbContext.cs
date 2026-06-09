using Microsoft.EntityFrameworkCore;
using CasinoAPI.Models;

namespace CasinoAPI.Data;

public class CasinoDbContext : DbContext
{
    public CasinoDbContext(DbContextOptions<CasinoDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<GameHistory> GameHistories => Set<GameHistory>();
}