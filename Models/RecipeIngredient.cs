using System;
using System.Collections.Generic;

namespace API.Models;

/// <summary>
/// Represents a recipe ingredient connection table
/// </summary>
public partial class RecipeIngredient
{
    /// <summary>
    /// The id of the recipe ingredient
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The Id of the linked recipe
    /// </summary>
    public int RecipeId { get; set; }

    /// <summary>
    /// The Id of the linked ingredient
    /// </summary>
    public int IngredientId { get; set; }

    /// <summary>
    /// The amount of the ingredient
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// The linked ingredient
    /// </summary>
    public virtual Ingredient Ingredient { get; set; } = null!;

    /// <summary>
    /// The linked recipe
    /// </summary>
    public virtual Recipe Recipe { get; set; } = null!;
}
