using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OlimpikonokAPI.Models;

public partial class OlimpikonokContext : DbContext
{
    public OlimpikonokContext()
    {
    }

    public OlimpikonokContext(DbContextOptions<OlimpikonokContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Orszag> Orszags { get; set; }

    public virtual DbSet<Sportag> Sportags { get; set; }

    public virtual DbSet<Sportolo> Sportolos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        optionsBuilder.UseMySQL(configuration.GetConnectionString("MyConnection"));        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Orszag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("orszag");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Fovaros).HasMaxLength(32);
            entity.Property(e => e.Nepesseg).HasColumnType("int(11)");
            entity.Property(e => e.Nev).HasMaxLength(32);
        });

        modelBuilder.Entity<Sportag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sportag");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Megnevezes).HasMaxLength(32);
        });

        modelBuilder.Entity<Sportolo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sportolo");

            entity.HasIndex(e => e.OrszagId, "OrszagId");

            entity.HasIndex(e => e.SportagId, "SportagId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Ermek).HasColumnType("int(11)");
            entity.Property(e => e.IndexKep).HasColumnType("blob");
            entity.Property(e => e.Kep).HasColumnType("mediumblob");
            entity.Property(e => e.Nev).HasMaxLength(32);
            entity.Property(e => e.OrszagId).HasColumnType("int(11)");
            entity.Property(e => e.SportagId).HasColumnType("int(11)");
            entity.Property(e => e.SzulDatum).HasColumnType("date");

            entity.HasOne(d => d.Orszag).WithMany(p => p.Sportolos)
                .HasForeignKey(d => d.OrszagId)
                .HasConstraintName("sportolo_ibfk_2");

            entity.HasOne(d => d.Sportag).WithMany(p => p.Sportolos)
                .HasForeignKey(d => d.SportagId)
                .HasConstraintName("sportolo_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
