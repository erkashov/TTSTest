using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TTSTest.TemplateDB;

public partial class TemplateDbContext : DbContext
{
    public TemplateDbContext()
    {
    }

    public TemplateDbContext(DbContextOptions<TemplateDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<ComponentType> ComponentTypes { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeMixerSet> RecipeMixerSets { get; set; }

    public virtual DbSet<RecipeStructure> RecipeStructures { get; set; }

    public virtual DbSet<RecipeTimeSet> RecipeTimeSets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=template_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
/*    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=DB\\template_db.db");*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__componen__3213E83F9A80B8CE");

            entity.ToTable("component");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Humidity).HasColumnName("humidity");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Type).WithMany(p => p.Components)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__component__type___49C3F6B7");
        });

        modelBuilder.Entity<ComponentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__componen__3213E83F8CE66A1C");

            entity.ToTable("component_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__recipe__3213E83F7BE70ABF");

            entity.ToTable("recipe");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.MixerSetId).HasColumnName("mixer_set_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.TimeSetId).HasColumnName("time_set_id");

            entity.HasOne(d => d.MixerSet).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.MixerSetId)
                .HasConstraintName("FK__recipe__mixer_se__4316F928");

            entity.HasOne(d => d.TimeSet).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.TimeSetId)
                .HasConstraintName("FK__recipe__time_set__440B1D61");
        });

        modelBuilder.Entity<RecipeMixerSet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__recipe_m__3213E83FBBBFFCE8");

            entity.ToTable("recipe_mixer_set");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UnloadTime)
                .HasDefaultValue(1)
                .HasColumnName("unload_time");
            entity.Property(e => e.UploadMode)
                .HasMaxLength(10)
                .HasDefaultValue("Constant")
                .HasColumnName("upload_mode");
        });

        modelBuilder.Entity<RecipeStructure>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.ComponentId }).HasName("PK__recipe_s__BF9AF03EFC31270F");

            entity.ToTable("recipe_structure");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.ComponentId).HasColumnName("component_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CorrectValue).HasColumnName("correct_value");

            entity.HasOne(d => d.Component).WithMany(p => p.RecipeStructures)
                .HasForeignKey(d => d.ComponentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recipe_st__compo__4E88ABD4");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeStructures)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__recipe_st__recip__4D94879B");
        });

        modelBuilder.Entity<RecipeTimeSet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__recipe_t__3213E83FA6F471D4");

            entity.ToTable("recipe_time_set");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MixTime).HasColumnName("mix_time");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
