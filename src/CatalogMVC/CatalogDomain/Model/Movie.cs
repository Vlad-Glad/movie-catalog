using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogDomain.Model;

public partial class Movie : Entity
{
    [Required(ErrorMessage = "Title field can't be empty!")]
    [Display(Name = "Movie title")]
    public string Title { get; set; } = null!;

    public int Year { get; set; }

    public string? Description { get; set; }

    [Display(Name = "Movie length in min")]

    public int MovieLength { get; set; }

    [Display(Name = "URL to poster in storage")]

    public string? Poster { get; set; }

    public virtual ICollection<DirectedBy> DirectedBies { get; set; } = new List<DirectedBy>();

    public virtual ICollection<MovieCast> MovieCasts { get; set; } = new List<MovieCast>();

    public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<ToWatchList> ToWatchLists { get; set; } = new List<ToWatchList>();

    public virtual ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();

    public virtual ICollection<WatchedList> WatchedLists { get; set; } = new List<WatchedList>();
}
