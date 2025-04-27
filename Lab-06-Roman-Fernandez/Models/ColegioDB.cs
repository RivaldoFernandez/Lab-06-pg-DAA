using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lab_06_Roman_Fernandez.Models;

public partial class ColegioDB : DbContext
{
    public ColegioDB()
    {
    }

    public ColegioDB(DbContextOptions<ColegioDB> options)
        : base(options)
    {
    }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Materium> Materia { get; set; }

    public virtual DbSet<Matricula> Matriculas { get; set; }

    public virtual DbSet<Nota> Notas { get; set; }

    public virtual DbSet<Profesor> Profesors { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=localhost;Database=lab06colegio;Uid=root;Pwd=123456;Port=3306");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Director>(entity =>
        {
            entity.HasKey(e => e.IdDirector).HasName("PRIMARY");

            entity.ToTable("director");

            entity.HasIndex(e => e.Correo, "Correo").IsUnique();

            entity.HasIndex(e => e.IdUsuario, "IdUsuario");

            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Directors)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("director_ibfk_1");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.IdEstudiante).HasName("PRIMARY");

            entity.ToTable("estudiante");

            entity.HasIndex(e => e.Correo, "Correo").IsUnique();

            entity.HasIndex(e => e.IdUsuario, "IdUsuario");

            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("estudiante_ibfk_1");
        });

        modelBuilder.Entity<Materium>(entity =>
        {
            entity.HasKey(e => e.IdMateria).HasName("PRIMARY");

            entity.ToTable("materia");

            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Matricula>(entity =>
        {
            entity.HasKey(e => e.IdMatricula).HasName("PRIMARY");

            entity.ToTable("matricula");

            entity.HasIndex(e => e.IdEstudiante, "IdEstudiante");

            entity.HasIndex(e => e.IdMateria, "IdMateria");

            entity.Property(e => e.Fecha).HasColumnType("date");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("matricula_ibfk_1");

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.IdMateria)
                .HasConstraintName("matricula_ibfk_2");
        });

        modelBuilder.Entity<Nota>(entity =>
        {
            entity.HasKey(e => e.IdNota).HasName("PRIMARY");

            entity.ToTable("notas");

            entity.HasIndex(e => e.IdEstudiante, "IdEstudiante");

            entity.HasIndex(e => e.IdMateria, "IdMateria");

            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.Nota1)
                .HasPrecision(5)
                .HasColumnName("Nota");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("notas_ibfk_1");

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdMateria)
                .HasConstraintName("notas_ibfk_2");
        });

        modelBuilder.Entity<Profesor>(entity =>
        {
            entity.HasKey(e => e.IdProfesor).HasName("PRIMARY");

            entity.ToTable("profesor");

            entity.HasIndex(e => e.Correo, "Correo").IsUnique();

            entity.HasIndex(e => e.IdUsuario, "IdUsuario");

            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Especialidad).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Profesors)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("profesor_ibfk_1");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "Correo").IsUnique();

            entity.Property(e => e.Clave).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Rol).HasColumnType("enum('Estudiante','Profesor','Director')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
