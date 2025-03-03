using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogDomain.Model;

public partial class Rating : Entity
{

    [Required(ErrorMessage = "This field can't be empty!")]
    public int MovieId { get; set; }

    [Required(ErrorMessage = "This field can't be empty!")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Minimum 3 symbols")]
    public string Source { get; set; } = null!;

    
    [Required(ErrorMessage = "This field can't be empty!")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Minimum 3 symbols")]
    public int Value { get; set; }

    public virtual Movie Movie { get; set; } = null!;
}
