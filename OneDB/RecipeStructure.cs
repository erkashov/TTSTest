using System;
using System.Collections.Generic;

namespace TTSTest.OneDB;

public partial class RecipeStructure
{
    public int RecipeId { get; set; }

    public int ComponentId { get; set; }

    public double Amount { get; set; }

    public virtual Component Component { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
