using System;
using System.Collections.Generic;

namespace TTSTest.TemplateDB;

public partial class RecipeMixerSet
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int UnloadTime { get; set; }

    public string UploadMode { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
