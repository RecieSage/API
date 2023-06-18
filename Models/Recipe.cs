using System;
using System.Collections.Generic;

namespace API.Models;

/// <summary>
/// Represents a recipe
/// </summary>
public partial class Recipe
{
    /// <summary>
    /// The id of the recipe
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the recipe
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The instructions of the recipe. Is in a markdown format
    /// </summary>
    public string Instructions { get; set; } = null!;

    /// <summary>
    /// Reprecents the linked <see cref="RecipeIngredient"/> table
    /// </summary>
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
