using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogDomain.Model;

public partial class WatchedList : Entity
{
    [Required(ErrorMessage = "This field can't be empty!")]
    [Range(0, int.MaxValue, ErrorMessage = "Can't be negative")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "This field can't be empty!")]
    [Range(0, int.MaxValue, ErrorMessage = "Can't be negative")]
    public int MovieId { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Watched Date")]
    [Required(ErrorMessage = "Watched date is required.")]
    public DateTime? WatchedDate { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
