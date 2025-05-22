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
        public string? FullName { get; set; }
    }
}

