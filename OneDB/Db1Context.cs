using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TTSTest.OneDB;

public partial class Db1Context : DbContext
{
    public Db1Context()
    {
    }

    public Db1Context(DbContextOptions<Db1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<ComponentType> ComponentTypes { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeStructure> RecipeStructures { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=db_1;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
   /* protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite("Data Source=DB\\db_1.db");*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__componen__3213E83FF35CD1D0");

            entity.ToTable("component");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Type).WithMany(p => p.Components)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__component__type___3D5E1FD2");
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__componen__3213E83F2D53FDF1");

            entity.ToTable("component_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__recipe__3213E83F839C3FE9");

            entity.ToTable("recipe");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.MixTime).HasColumnName("mix_time");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.WaterCorrect).HasColumnName("water_correct");
        });

        modelBuilder.Entity<RecipeStructure>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.ComponentId }).HasName("PK__recipe_s__BF9AF03EED3D2A4D");

            entity.ToTable("recipe_structure");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.ComponentId).HasColumnName("component_id");
            entity.Property(e => e.Amount).HasColumnName("amount");

            entity.HasOne(d => d.Component).WithMany(p => p.RecipeStructures)
                .HasForeignKey(d => d.ComponentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recipe_st__compo__412EB0B6");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeStructures)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recipe_st__recip__403A8C7D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
