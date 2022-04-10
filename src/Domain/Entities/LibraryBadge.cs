using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LibraryBadge
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expired { get; set; }
    }
}
