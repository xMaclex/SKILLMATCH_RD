using Microsoft.EntityFrameworkCore;
using SKILLMATCH_RD.Models;

namespace SKILLMATCH_RD.Data;

// Contexto de Entity Framework Core (Avance 3).
// Reemplaza las listas estaticas en memoria del Avance 2 por persistencia real en SQLite.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Universidad> Universidades => Set<Universidad>();
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Oferta> Ofertas => Set<Oferta>();
    public DbSet<Aptitud> Aptitudes => Set<Aptitud>();
    public DbSet<Solicitud> Solicitudes => Set<Solicitud>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Empresa 1 --- N Ofertas
        modelBuilder.Entity<Oferta>()
            .HasOne(o => o.Empresa)
            .WithMany(e => e.Ofertas)
            .HasForeignKey(o => o.EmpresaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Universidad 1 --- N Estudiantes
        modelBuilder.Entity<Estudiante>()
            .HasOne(e => e.Universidad)
            .WithMany(u => u.Estudiantes)
            .HasForeignKey(e => e.UniversidadId)
            .OnDelete(DeleteBehavior.Restrict);

        // Solicitud N --- 1 Estudiante / N --- 1 Oferta
        modelBuilder.Entity<Solicitud>()
            .HasOne(s => s.Estudiante)
            .WithMany(e => e.Solicitudes)
            .HasForeignKey(s => s.EstudianteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Solicitud>()
            .HasOne(s => s.Oferta)
            .WithMany(o => o.Solicitudes)
            .HasForeignKey(s => s.OfertaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relaciones muchos-a-muchos con Aptitudes
        modelBuilder.Entity<Oferta>()
            .HasMany(o => o.AptitudesRequeridas)
            .WithMany(a => a.Ofertas);

        modelBuilder.Entity<Estudiante>()
            .HasMany(e => e.Aptitudes)
            .WithMany(a => a.Estudiantes);
    }
}
