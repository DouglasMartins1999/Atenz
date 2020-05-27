using Atenz.Repository.Repositories;
using Atenz.Domain.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Atenz.API.Helpers;

namespace Atenz.API.Controllers
{
    [Route("courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly StorageService storage;
        private readonly CourseRepository repository;
        public CoursesController(CourseRepository courseRepository, IMapper mapper, StorageService storage)
        {
            this.repository = courseRepository;
            this.mapper = mapper;
            this.storage = storage;
        }

        [Authorize]
        [Route("{id:long}")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await repository.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("{id:long}/favorite")]
        public async Task<ActionResult> AddToFavorites(long id)
        {
            var user = long.Parse(User.Claims.First().Value);
            var result = await repository.AddToFavorites(user, id);
            return Ok(new { added = result });
        }

        [Authorize]
        [Route("module/{id:long}")]
        public async Task<ActionResult> GetModule(long id)
        {
            var lessons = await repository.GetLessonByModule(id);
            var result = mapper.Map<IEnumerable<ModuleLessonDTO>>(lessons);
            return Ok(result);
        }

        [Authorize]
        [Route("module/lesson/{id:long}")]
        public async Task<ActionResult> GetLesson(long id)
        {
            var user = long.Parse(User.Claims.First().Value);
            var lesson = await repository.GetLessonById(id);
            var link = await storage.GetPreSignedLink(lesson.Link);
            var metadata = await storage.GetObjectInfo(lesson.Link);
            var result = mapper.Map<LessonDTO>(lesson);

            result.Link = link;
            result.Size = metadata.Size;
            await repository.AddToHistory(user, id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("module/lesson/{id:long}/watchlater")]
        public async Task<ActionResult> AddToWatchLater(long id)
        {
            var user = long.Parse(User.Claims.First().Value);
            var result = await repository.AddToWatchLater(user, id);
            return Ok(new { added = result });
        }

        [Authorize]
        [Route("search")]
        public async Task<ActionResult> Search([FromQuery] string q, [FromQuery] int pag = 1){
            var user = long.Parse(User.Claims.First().Value);
            var result = await repository.Query(q, user, pag);
            return Ok(result);
        }

        [Authorize]
        [Route("search/featured")]
        public async Task<ActionResult> Featured([FromQuery] string q){
            var user = long.Parse(User.Claims.First().Value);
            var result = await repository.QueryFeatured(q, user);
            return Ok(result);
        }
    }
}