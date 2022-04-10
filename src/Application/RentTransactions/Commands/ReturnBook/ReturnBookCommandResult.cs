using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RentTransactions.Commands.ReturnBook
{
    public class ReturnBookCommandResult
    {
        public decimal Penalty { get; set; }
        public Currency Currency { get; set; }
    }
}
