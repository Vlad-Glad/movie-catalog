using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogDomain.Model;

public partial class ToWatchList : Entity
{
    [Required(ErrorMessage = "This field can't be empty!")]
    [Range(0, int.MaxValue, ErrorMessage = "Can't be negative")]
    public string UserId { get; set; } = null!;

    [Required(ErrorMessage = "This field can't be empty!")]
    [Range(0, int.MaxValue, ErrorMessage = "Can't be negative")]
    public int MovieId { get; set; }

    public DateTime? AddedDate { get; set; }

    public virtual Movie Movie { get; set; } = null!;
}
