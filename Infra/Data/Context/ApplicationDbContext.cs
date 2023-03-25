using ApiAluguelCavalos.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiAluguelCavalos.Infra.Data.Context;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<CavaloModel> Cavalos { get; set; }
    public DbSet<AluguelModel> Alugueis { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CavaloModel>()
            .Property(c => c.Nome).IsRequired();
        builder.Entity<CavaloModel>()
            .Property(c => c.Registro).IsRequired();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
    {
        configuration.Properties<string>()
            .HaveMaxLength(100);
    }
}
