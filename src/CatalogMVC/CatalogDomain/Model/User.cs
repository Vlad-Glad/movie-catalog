using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogDomain.Model;

public partial class User : Entity
{
    [Required(ErrorMessage ="Email is required")]
    [EmailAddress(ErrorMessage ="Invalid email format")]
    public string Email { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    [NotMapped]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }


    [NotMapped]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }

    public string Role { get; set; } = null!;

    [NotMapped]
    public const string Admin = "Admin";
    [NotMapped]
    public const string OrdinaryUser = "User";

    public virtual ICollection<ToWatchList> ToWatchLists { get; set; } = new List<ToWatchList>();

    public virtual ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();

    public virtual ICollection<WatchedList> WatchedLists { get; set; } = new List<WatchedList>();
}
