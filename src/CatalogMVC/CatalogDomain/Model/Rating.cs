using System;
using System.Collections.Generic;

namespace CatalogDomain.Model;

public partial class Rating : Entity
{
    public int MovieId { get; set; }

    public string Source { get; set; } = null!;

    public int Value { get; set; }

    public virtual Movie Movie { get; set; } = null!;
}
