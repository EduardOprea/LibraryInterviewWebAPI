using System;

namespace Application.RentTransactions.Commands.RentBook
{
    public class RentBookCommandResult
    {
        public int RentTransactionId { get; set; }
        public DateTime MustReturnDate { get; set; }
    }
}
