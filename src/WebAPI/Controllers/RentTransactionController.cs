using Application.RentTransactions.Commands.RentBook;
using Application.RentTransactions.Commands.ReturnBook;
using Application.RentTransactions.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentTransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RentTransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<List<RentTransactionDto>>> GetAllRentTransactions()
        {
            var query = new GetAllRentTransactionsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("rent-book")]
        public async Task<ActionResult<RentBookCommandResult>> RentBook([FromBody] RentBookCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("return-book")]
        public async Task<ActionResult<ReturnBookCommandResult>> ReturnBook([FromBody] ReturnBookCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
