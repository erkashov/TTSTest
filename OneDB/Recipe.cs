using System;
using System.Collections.Generic;

namespace TTSTest.OneDB;

public partial class Recipe
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DateModified { get; set; }

    public int MixTime { get; set; }

    public double WaterCorrect { get; set; }

    public virtual ICollection<RecipeStructure> RecipeStructures { get; set; } = new List<RecipeStructure>();
}
