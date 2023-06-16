using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Ingredient
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
