using System.Linq;
using System.Threading.Tasks;
using Atenz.API.Helpers;
using Atenz.Repository.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atenz.API.Controllers
{
    [Route("books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookRepository repository;
        private readonly StorageService storage;
        private readonly IMapper mapper;

        public BooksController(BookRepository bookRepository, IMapper mapper, StorageService storage)
        {
            this.storage = storage;
            this.mapper = mapper;
            this.repository = bookRepository;
        }

        [Authorize]
        [Route("{id:long}")]
        public async Task<ActionResult> GetOne(long id)
        {
            var result = await repository.GetById(id);
            var link = await storage.GetPreSignedLink(result.Link, "books");

            result.Link = link;
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("{id:long}/favorite")]
        public async Task<ActionResult> MarkAsFavorite(long id)
        {
            var user = long.Parse(User.Claims.First().Value);
            var result = await repository.MarkAsFavorite(id, user);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("{id:long}/read")]
        public async Task<ActionResult> MarkAsReaded(long id)
        {
            var user = long.Parse(User.Claims.First().Value);
            var result = await repository.MarkAsReaded(id, user);
            return Ok(result);
        }

        [Authorize]
        [Route("search")]
        public async Task<ActionResult> Search([FromQuery] string q, [FromQuery] int pag = 1)
        {
            var user = long.Parse(User.Claims.First().Value);
            var result = await repository.Query(q, user, pag);
            return Ok(result);
        }
    }
}