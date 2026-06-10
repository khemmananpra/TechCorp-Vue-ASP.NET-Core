using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TechCorp_Vue.Server.Models;

public partial class TechCorpContext : DbContext
{
    public TechCorpContext()
    {
    }

    public TechCorpContext(DbContextOptions<TechCorpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Filemodel> Filemodels { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Signature> Signatures { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-7BO34V39\\SQLEXPRESS;Initial Catalog=TechCorp;Integrated Security=True;Pooling=False;Encrypt=False;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasOne(d => d.Department).WithMany(p => p.Employees).HasConstraintName("FK_Employee_Department");

            entity.HasOne(d => d.IdfileNavigation).WithMany(p => p.Employees).HasConstraintName("FK_Employee_Filemodel");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager).HasConstraintName("FK_Employee_Employee");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees).HasConstraintName("FK_Employee_Position");

            entity.HasOne(d => d.Signature).WithMany(p => p.Employees).HasConstraintName("FK_Employee_signature");
        });

        modelBuilder.Entity<Filemodel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_File");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
