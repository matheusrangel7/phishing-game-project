using Game.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Game.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Email> Emails { get; set; } // Representa a tabela de emails no banco
}