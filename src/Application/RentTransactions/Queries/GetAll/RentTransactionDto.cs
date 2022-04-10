using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RentTransactions.Queries.GetAll
{
    public class RentTransactionDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int LibraryBadgeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime MustReturnDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal RentPrice { get; set; }
        public Currency Currency { get; set; }
    }
}
