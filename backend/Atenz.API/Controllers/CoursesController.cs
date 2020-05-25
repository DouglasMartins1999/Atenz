using Atenz.Repository.Repositories;
using Atenz.Domain.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;

namespace Atenz.API.Controllers
{
    [Route("courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly CourseRepository repository;
        public CoursesController(CourseRepository courseRepository, IMapper mapper)
        {
            this.repository = courseRepository;
            this.mapper = mapper;
        }

        [Route("{id:long}")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await repository.GetById(id);
            return Ok(result);
        }

        [Route("module/{id:long}")]
        public async Task<ActionResult> GetModule(long id)
        {
            var lessons = await repository.GetLessonByModule(id);
            var result = mapper.Map<IEnumerable<ModuleLessonDTO>>(lessons);
            return Ok(result);
        }

        [Route("module/lesson/{id:long}")]
        public async Task<ActionResult> GetLesson(long id)
        {
            var lesson = await repository.GetLessonById(id);
            var result = mapper.Map<LessonDTO>(lesson);
            return Ok(result);
        }
    }
}