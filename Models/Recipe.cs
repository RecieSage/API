using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Recipe
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Instructions { get; set; } = null!;
}
