using Application.Books.Commands.CreateBook;
using Application.Books.Queries.GetAllBooks;
using Application.Books.Queries.GetBooksCountByISBN;
using Application.Books.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<List<BookDto>>> GetAllBooks()
        {
            var query = new GetAllBooksQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            var query = new GetByIdQuery{ Id = id};
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result}, result);
        }

        [HttpGet("count-by-isbn")]
        public async Task<ActionResult<int>> GetBooksCountByISBN([FromQuery] string ISBN)
        {
            var query = new GetBooksCountByISBNQuery(ISBN);
            var result = await _mediator.Send(query);
            return Ok(result);
        }


    }
}
