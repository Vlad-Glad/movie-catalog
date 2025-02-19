using System;
using System.Collections.Generic;

namespace CatalogDomain.Model;

public partial class User : Entity
{
    public string Email { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<ToWatchList> ToWatchLists { get; set; } = new List<ToWatchList>();

    public virtual ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();

    public virtual ICollection<WatchedList> WatchedLists { get; set; } = new List<WatchedList>();
}
