using System;
using System.Collections.Generic;

namespace TTSTest.TwoDB;

public partial class TimeSet
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int MixTime { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
