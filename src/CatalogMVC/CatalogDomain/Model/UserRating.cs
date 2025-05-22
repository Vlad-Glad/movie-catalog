using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogDomain.Model;

public partial class UserRating : Entity
{
    [Required(ErrorMessage = "This field can't be empty!")]
    public string UserId { get; set; } = null!;

    [Required(ErrorMessage = "This field can't be empty!")]
    [Range(0, int.MaxValue, ErrorMessage = "Can't be negative")]
    public int MovieId { get; set; }

    [Required(ErrorMessage = "This field can't be empty!")]
    [Range(0, 10, ErrorMessage = "Score can be in ragne from 0 to 10")]
    [Display(Name = "Score")]
    public byte? Value { get; set; }

    public virtual Movie Movie { get; set; } = null!;
}
