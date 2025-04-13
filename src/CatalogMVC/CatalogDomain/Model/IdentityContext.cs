using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CatalogDomain.Model
{
    public class IdentityContext : IdentityDbContext<AspNetCoreUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
    : base(options)
        {
        }
    }
}
