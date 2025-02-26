using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogDomain.Model;

public partial class Movie : Entity
{
    [Required(ErrorMessage = "Title field can't be empty!")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
    [Display(Name = "Movie title")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Year field can't be empty!")]
    public int Year { get; set; }

    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Title field can't be empty!")]
    [Range(1, 1000000, ErrorMessage = "Length can't be less than 1 minute ")]
    [Display(Name = "Movie length in min")]
    public int MovieLength { get; set; }

    [Url(ErrorMessage = "Invalid URL format.")]
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
