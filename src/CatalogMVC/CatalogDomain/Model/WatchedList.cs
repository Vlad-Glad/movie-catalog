using System;
using System.Collections.Generic;

namespace CatalogDomain.Model;

public partial class WatchedList : Entity
{
    public int UserId { get; set; }

    public int MovieId { get; set; }

    public DateTime? WatchedDate { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
