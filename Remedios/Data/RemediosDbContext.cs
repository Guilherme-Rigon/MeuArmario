using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Remedios.Models;
using Remedios.ViewModel;

namespace Remedios.Data
{
    public class RemediosDbContext : DbContext
    {
        public RemediosDbContext(DbContextOptions<RemediosDbContext> options) : base(options)
        {
        }

        public DbSet<MembroFamilia> MembrosFamilia { get; set; }
        public DbSet<Remedio> Remedios { get; set; }
        public DbSet<Receita> Receitas { get; set; }
        public DbSet<MembroRemedio> MembroRemedios { get; set; }
        public DbSet<Foto> Fotos { get; set; }
        public DbSet<Dose> Doses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MembroRemedio>().HasKey(sc => new { sc.RemedioId, sc.UserId });
        }
    }
}
