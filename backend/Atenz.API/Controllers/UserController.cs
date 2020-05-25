using System.Collections.Generic;
using System.Threading.Tasks;
using Atenz.Domain.DTOs;
using Atenz.Repository.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Atenz.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository repository;
        private readonly IMapper mapper;

        public UserController(UserRepository userRepository, IMapper Mapper)
        {
            this.repository = userRepository;
            this.mapper = Mapper;
        }

        [Route("profile/{id:long}")]
        public async Task<ActionResult> Get(long id)
        {
            var user = await repository.ProfileBasic(id);
            var latest = await repository.LatestWatched(id);
            var recentCourse = await repository.RecentsCourses(id);
            var favoriteCourse = await repository.FavoriteCourses(id);
            var watchLater = await repository.LessonsToWatchLater(id);
            var interests = await repository.Interests(id);
            var statistics = await repository.Statistics(id);
            var goals = await repository.Goals(id);
            
            var result = mapper.Map<ProfileDTO>(user);
            result.Latest = mapper.Map<Latest>(latest);
            result.RecentCourses = mapper.Map<List<MinimalCourse>>(recentCourse);
            result.FavCourses = mapper.Map<List<MinimalCourse>>(favoriteCourse);
            result.WatchLater = mapper.Map<List<MinimalLesson>>(watchLater);
            result.Interests = interests;
            result.Statistics = statistics;
            result.Goals = goals;

            return Ok(result);
        }
    }
}