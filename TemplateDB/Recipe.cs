using System;
using System.Collections.Generic;

namespace TTSTest.TemplateDB;

public partial class Recipe
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DateModified { get; set; }

    public int? MixerSetId { get; set; }

    public int? TimeSetId { get; set; }

    public virtual RecipeMixerSet? MixerSet { get; set; }

    public virtual ICollection<RecipeStructure> RecipeStructures { get; set; } = new List<RecipeStructure>();

    public virtual RecipeTimeSet? TimeSet { get; set; }
}
