using System;
using System.Collections.Generic;

namespace TTSTest.TwoDB;

public partial class Recipe
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DateModified { get; set; }

    public int? MixerSetId { get; set; }

    public int? TimeSetId { get; set; }

    public int? ConsistencyId { get; set; }

    public virtual Consistency? Consistency { get; set; }

    public virtual MixerSet? MixerSet { get; set; }

    public virtual TimeSet? TimeSet { get; set; }
}
