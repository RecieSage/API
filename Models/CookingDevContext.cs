using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class CookingDevContext : DbContext
{
    public CookingDevContext()
    {
    }

    public CookingDevContext(DbContextOptions<CookingDevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.ToTable("Ingredient");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("unit");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.ToTable("Recipe");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Instructions)
                .HasMaxLength(1500)
                .HasColumnName("instructions");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.IngredientId });

            entity.ToTable("Recipe_Ingredient");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.Amount).HasColumnName("amount");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recipe_Ingredient_Ingredient");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recipe_Ingredient_Recipe");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
