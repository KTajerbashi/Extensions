using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DatabaseModelScaffold.Models;

public partial class ChangeDataLogDbContext : DbContext
{
    public ChangeDataLogDbContext()
    {
    }

    public ChangeDataLogDbContext(DbContextOptions<ChangeDataLogDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EntityChangeDataLog> EntityChangeDataLogs { get; set; }

    public virtual DbSet<PropertyChangeDataLog> PropertyChangeDataLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server =TAJERBASHI; Database=ChangeDataLog_Db;User Id = sa;Password=123123; MultipleActiveResultSets=true; Encrypt = false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityChangeDataLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EntityCh__3214EC071EB59924");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ChangeType).HasMaxLength(50);
            entity.Property(e => e.ContextName).HasMaxLength(200);
            entity.Property(e => e.DateOfOccurrence).HasColumnType("datetime");
            entity.Property(e => e.EntityId).HasMaxLength(200);
            entity.Property(e => e.EntityType).HasMaxLength(200);
            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .HasColumnName("IP");
            entity.Property(e => e.TransactionId).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(200);
        });

        modelBuilder.Entity<PropertyChangeDataLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Property__3214EC078417E097");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.PropertyName).HasMaxLength(200);

            entity.HasOne(d => d.ChangeInterceptorItem).WithMany(p => p.PropertyChangeDataLogs)
                .HasForeignKey(d => d.ChangeInterceptorItemId)
                .HasConstraintName("FK__PropertyC__Chang__3C69FB99");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users", "Security");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
