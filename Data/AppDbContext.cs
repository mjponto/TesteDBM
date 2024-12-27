using Microsoft.EntityFrameworkCore;
using TesteDBM.Models;

namespace TesteDBM.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>().Property(p => p.Nome)
            .HasMaxLength(100)
            .IsRequired();
            
        modelBuilder.Entity<Produto>().Property(p => p.Preco)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
            
        modelBuilder.Entity<Produto>().Property(p => p.DataCadastro)
            .IsRequired();
    }
}