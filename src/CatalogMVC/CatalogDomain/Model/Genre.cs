using System;
using System.Collections.Generic;

namespace CatalogDomain.Model;

public partial class Genre : Entity
{
    public string GenreName { get; set; } = null!;

    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
}
