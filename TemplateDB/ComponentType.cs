using System;
using System.Collections.Generic;

namespace TTSTest.TemplateDB;

public partial class ComponentType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Component> Components { get; set; } = new List<Component>();
}
