using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Ingredient
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Amount { get; set; }

    public string Unit { get; set; } = null!;
}
