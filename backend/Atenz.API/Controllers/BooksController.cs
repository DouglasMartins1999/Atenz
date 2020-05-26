using System.Threading.Tasks;
using Atenz.Repository.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Atenz.API.Controllers
{
    [Route("books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookRepository repository;
        private readonly IMapper mapper;

        public BooksController(BookRepository bookRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = bookRepository;
        }

        [Route("{id:long}")]
        public async Task<ActionResult> GetOne(long id)
        {
            var result = await repository.GetById(id);
            return Ok(result);
        }

        [Route("{id:long}/favorite")]
        public async Task<ActionResult> MarkAsFavorite(long id, [FromQuery] long user)
        {
            var result = await repository.MarkAsFavorite(id, user);
            return Ok(result);
        }

        [Route("{id:long}/read")]
        public async Task<ActionResult> MarkAsReaded(long id, [FromQuery] long user)
        {
            var result = await repository.MarkAsReaded(id, user);
            return Ok(result);
        }

        [Route("search")]
        public async Task<ActionResult> Search([FromQuery] string q, [FromQuery] long user, [FromQuery] int pag = 1)
        {
            var result = await repository.Query(q, user, pag);
            return Ok(result);
        }
    }
}