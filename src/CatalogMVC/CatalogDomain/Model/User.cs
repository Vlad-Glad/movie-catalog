using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogDomain.Model;

public partial class User : Entity
{
    [Required(ErrorMessage ="Email is required")]
    [EmailAddress(ErrorMessage ="Invalid email format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage ="Password is reqired")]
    public byte[] PasswordHash { get; set; } = null!;

    [Required(ErrorMessage = "Role is required.")]
    [RegularExpression("^(Admin|User)$", ErrorMessage = "Invalid role. Allowed values: Admin, User, Moderator.")]
    public string Role { get; set; } = null!;

    public virtual ICollection<ToWatchList> ToWatchLists { get; set; } = new List<ToWatchList>();

    public virtual ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();

    public virtual ICollection<WatchedList> WatchedLists { get; set; } = new List<WatchedList>();
}
