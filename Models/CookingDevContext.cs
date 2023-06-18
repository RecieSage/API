using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

/// <summary>
/// Database context manager
/// </summary>
public partial class CookingDevContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CookingDevContext"/> class.
    /// </summary>
    public CookingDevContext()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CookingDevContext"/> class.
    /// </summary>
    /// <param name="options">Instance of <see cref="DbContextOptions"/></param>
    public CookingDevContext(DbContextOptions<CookingDevContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the <see cref="Ingredient"/> table
    /// </summary>
    public virtual DbSet<Ingredient> Ingredients { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Recipe"/> table
    /// </summary>
    public virtual DbSet<Recipe> Recipes { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="RecipeIngredient"/> table
    /// </summary>
    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING"));

    /// <inheritdoc/>
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
                .HasMaxLength(50)
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
            entity.ToTable("Recipe_Ingredient");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.IngredientId)
                .HasConstraintName("FK_Recipe_Ingredient_Ingredient");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("FK_Recipe_Ingredient_Recipe");
        });

        this.OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
