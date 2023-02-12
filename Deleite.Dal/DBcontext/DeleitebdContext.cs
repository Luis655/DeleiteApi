using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Deleite.Entity.Models;

namespace Deleite.Dal.DBContext;

public partial class DeleitebdContext : DbContext
{
    public DeleitebdContext()
    {
    }

    public DeleitebdContext(DbContextOptions<DeleitebdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Tematica> Tematicas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK_IDCATEGORIA");

            entity.ToTable("CATEGORIAS");

            entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");
            entity.Property(e => e.Imagen)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGEN");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK_ID_PRODUCTO");

            entity.ToTable("PRODUCTO");

            entity.Property(e => e.IdProducto).HasColumnName("ID_PRODUCTO");
            entity.Property(e => e.DescripcionP)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION_P");
            entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");
            entity.Property(e => e.IdConfirmacionT).HasColumnName("ID_CONFIRMACION_T");
            entity.Property(e => e.IdTematica).HasColumnName("ID_TEMATICA");
            entity.Property(e => e.ImagenPrincipal)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGEN_PRINCIPAL");
            entity.Property(e => e.Ingredienteselect)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("INGREDIENTESELECT");
            entity.Property(e => e.NombreP)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_P");
            entity.Property(e => e.Popular).HasColumnName("POPULAR");
            entity.Property(e => e.Precio)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PRECIO");
            entity.Property(e => e.Saludable).HasColumnName("SALUDABLE");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ID_CATEGORIA");

            entity.HasOne(d => d.IdTematicaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdTematica)
                .HasConstraintName("FK_ID_TEMATICA");
        });

        modelBuilder.Entity<Tematica>(entity =>
        {
            entity.HasKey(e => e.IdTematica).HasName("PK_ID_TEMATICA");

            entity.ToTable("TEMATICA");

            entity.Property(e => e.IdTematica).HasColumnName("ID_TEMATICA");
            entity.Property(e => e.NombreT)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_T");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("ID_USUARIO");

            entity.ToTable("USUARIO");

            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.Contraseña)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONTRASEÑA");
            entity.Property(e => e.Correo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CORREO");
            entity.Property(e => e.FechaToken)
                .HasColumnType("datetime")
                .HasColumnName("FECHA_TOKEN");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Token)
                .IsUnicode(false)
                .HasColumnName("TOKEN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
