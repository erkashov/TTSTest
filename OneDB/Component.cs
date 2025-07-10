using System;
using System.Collections.Generic;

namespace TTSTest.OneDB;

public partial class Component
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TypeId { get; set; }

    public virtual ICollection<RecipeStructure> RecipeStructures { get; set; } = new List<RecipeStructure>();

    public virtual ComponentType Type { get; set; } = null!;
}
