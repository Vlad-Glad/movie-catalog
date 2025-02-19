using System;
using System.Collections.Generic;

namespace CatalogDomain.Model;

public partial class Actor : Entity
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    public virtual ICollection<MovieCast> MovieCasts { get; set; } = new List<MovieCast>();
}
