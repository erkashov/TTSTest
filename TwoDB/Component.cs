using System;
using System.Collections.Generic;

namespace TTSTest.TwoDB;

public partial class Component
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TypeId { get; set; }

    public double Humidity { get; set; }

    public virtual ComponentType Type { get; set; } = null!;
}
