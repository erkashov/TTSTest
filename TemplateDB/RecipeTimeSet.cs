using System;
using System.Collections.Generic;

namespace TTSTest.TemplateDB;

public partial class RecipeTimeSet
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int MixTime { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
