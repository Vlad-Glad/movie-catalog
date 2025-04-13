using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CatalogDomain.Model
{
   public class AspNetCoreUser: IdentityUser
   {
        public virtual ICollection<ToWatchList> ToWatchLists { get; set; } = new List<ToWatchList>();
        public virtual ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();
        public virtual ICollection<WatchedList> WatchedLists { get; set; } = new List<WatchedList>();
   }
}

