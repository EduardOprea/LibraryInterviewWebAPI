using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Books.Queries.GetAllBooks
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal BaseRentPrice { get; set; }
        public Currency Currency { get; set; }
        public string ISBN { get; set; }
    }
}
