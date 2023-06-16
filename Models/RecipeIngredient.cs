using System;
using System.Collections.Generic;

namespace API.Models;

public partial class RecipeIngredient
{
    public int RecipeId { get; set; }

    public int IngredientId { get; set; }
}
