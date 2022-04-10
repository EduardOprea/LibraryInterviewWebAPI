using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RentTransaction
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int LibraryBadgeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime MustReturnDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal RentPrice { get; set; }
        public Currency Currency { get; set; }
        public virtual Book Book { get; set; }
        public virtual LibraryBadge LibraryBadge { get; set; }
    }
}
