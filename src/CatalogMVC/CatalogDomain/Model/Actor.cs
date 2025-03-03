using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogDomain.Model;

public partial class Actor : Entity
{
    [Required(ErrorMessage = "First name field can't be empty!")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Minimum 3 symbols")]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name field can't be empty!")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Minimum 3 symbols")]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Nationality field can't be empty!")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Minimum 3 symbols")]
    [Display(Name = "Nationality")]
    public string Nationality { get; set; } = null!;

    public virtual ICollection<MovieCast> MovieCasts { get; set; } = new List<MovieCast>();
}
