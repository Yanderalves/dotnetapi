using Microsoft.EntityFrameworkCore;
namespace AmigoSecreto.Models;

public class AmigoSecretoContext : DbContext
{
    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Matches> Matches { get; set; }

    public AmigoSecretoContext(DbContextOptions<AmigoSecretoContext> options): base(options) 
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pessoa>().HasIndex(w => w.Email).IsUnique();

        modelBuilder.Entity<Matches>()
            .HasOne(i => i.Sender)
            .WithMany()
            .HasForeignKey(i => i.SenderId);
        
        modelBuilder.Entity<Matches>()
            .HasOne(i => i.Receiver)
            .WithMany()
            .HasForeignKey(i => i.ReceiverId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql("Server=192.168.0.44;Port=5432;Database=postgres;User Id=postgres;Password=postgres;");

}