using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogDomain.Model
{
    [Keyless]
    public class ActorPair
    {
        public int Actor1Id { get; set; }
        public string Actor1FirstName { get; set; }
        public string Actor1LastName { get; set; }

        public int Actor2Id { get; set; }
        public string Actor2FirstName { get; set; }
        public string Actor2LastName { get; set; }

        public string Movies { get; set; }
    }
}
