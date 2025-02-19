using System;
using System.Collections.Generic;

namespace CatalogDomain.Model;

public partial class DirectedBy : Entity
{
    public int MovieId { get; set; }

    public int DirectorId { get; set; }

    public virtual Director Director { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
