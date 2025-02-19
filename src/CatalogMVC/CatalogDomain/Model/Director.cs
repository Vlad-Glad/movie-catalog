using System;
using System.Collections.Generic;

namespace CatalogDomain.Model;

public partial class Director : Entity
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    public virtual ICollection<DirectedBy> DirectedBies { get; set; } = new List<DirectedBy>();
}
