using System;
using System.Collections.Generic;

namespace CatalogDomain.Model;

public partial class MovieCast : Entity
{
    public int MovieId { get; set; }

    public int ActorId { get; set; }

    public virtual Actor Actor { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
