using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TTSTest.TwoDB;

public partial class Db2Context : DbContext
{
    public Db2Context()
    {
    }

    public Db2Context(DbContextOptions<Db2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<ComponentType> ComponentTypes { get; set; }

    public virtual DbSet<Consistency> Consistencies { get; set; }

    public virtual DbSet<MixerSet> MixerSets { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<TimeSet> TimeSets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=db_2;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    /*protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite("Data Source=DB\\db_2.db");*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__componen__3213E83F77876570");

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
            entity.HasKey(e => e.Id).HasName("PK__componen__3213E83FB4B927EF");

            entity.ToTable("component_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Consistency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__consiste__3213E83F1ECAD44E");

            entity.ToTable("consistency");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MixerSet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mixer_se__3213E83F236CC9E3");

            entity.ToTable("mixer_set");

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

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__recipe__3213E83F55BD0A34");

            entity.ToTable("recipe");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConsistencyId).HasColumnName("consistency_id");
            entity.Property(e => e.DateModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.MixerSetId).HasColumnName("mixer_set_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.TimeSetId).HasColumnName("time_set_id");

            entity.HasOne(d => d.Consistency).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.ConsistencyId)
                .HasConstraintName("FK__recipe__consiste__6754599E");

            entity.HasOne(d => d.MixerSet).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.MixerSetId)
                .HasConstraintName("FK__recipe__mixer_se__656C112C");

            entity.HasOne(d => d.TimeSet).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.TimeSetId)
                .HasConstraintName("FK__recipe__time_set__66603565");
        });

        modelBuilder.Entity<TimeSet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__time_set__3213E83F5F76EF6B");

            entity.ToTable("time_set");

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
