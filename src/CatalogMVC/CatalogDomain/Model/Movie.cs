using System;
using System.Collections.Generic;

namespace CatalogDomain.Model;

public partial class Movie : Entity
{
    public string Title { get; set; } = null!;

    public int Year { get; set; }

    public string? Description { get; set; }

    public int MovieLength { get; set; }

    public string? Poster { get; set; }

    public virtual ICollection<DirectedBy> DirectedBies { get; set; } = new List<DirectedBy>();

    public virtual ICollection<MovieCast> MovieCasts { get; set; } = new List<MovieCast>();

    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<ToWatchList> ToWatchLists { get; set; } = new List<ToWatchList>();

    public virtual ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();

    public virtual ICollection<WatchedList> WatchedLists { get; set; } = new List<WatchedList>();
}
