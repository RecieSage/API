using System;
using System.Collections.Generic;

namespace API.Models;

/// <summary>
/// Represents an ingredient
/// </summary>
public partial class Ingredient
{
    /// <summary>
    /// The id of the ingredient
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the ingredient
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The unit of the ingredient
    /// </summary>
    public string Unit { get; set; } = null!;

    /// <summary>
    /// Reprecents the linked <see cref="RecipeIngredient"/> table
    /// </summary>
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
