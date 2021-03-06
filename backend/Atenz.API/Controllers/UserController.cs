using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atenz.API.Helpers;
using Atenz.Domain.DTOs;
using Atenz.Domain.Identity;
using Atenz.Repository.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        [AllowAnonymous]
        [Route("signin")]
        public async Task<ActionResult> Login(User u)
        {
            try {
                var user = await repository.Auth(u.Username, u.Password);
                var result = new { token = TokenService.GenerateToken(user) };
                return Ok(result);

            } catch(Exception e){
                Console.WriteLine(e.Message);
                return Forbid();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("signup")]
        public async Task<ActionResult> SignIn(User u)
        {
            var user = await repository.Register(u);
            var result = new { token = TokenService.GenerateToken(user) };
            return Ok(result);
        }

        [Authorize]
        [Route("profile")]
        public async Task<ActionResult> Get()
        {
            var id = long.Parse(User.Claims.First().Value);

            var user = await repository.ProfileBasic(id);
            var statistics = await repository.Statistics(id);
            var interests = await repository.Interests(id);
            var goals = await repository.Goals(id);
            var latest = await repository.LatestWatched(id);
            var recentCourse = await repository.RecentsCourses(id, 1);
            var favoriteCourse = await repository.FavoriteCourses(id, 1);
            var watchLater = await repository.LessonsToWatchLater(id, 1);
            var recentBooks = await repository.RecentsBooks(id, 1);
            var favoriteBooks = await repository.FavoriteBooks(id, 1);
            
            var result = mapper.Map<ProfileDTO>(user);
            result.Latest = mapper.Map<Latest>(latest);
            result.RecentCourses = recentCourse;
            result.FavCourses = mapper.Map<List<MinimalCourse>>(favoriteCourse);
            result.WatchLater = mapper.Map<List<MinimalLesson>>(watchLater);
            result.Interests = interests;
            result.Statistics = statistics;
            result.Goals = goals;
            result.RecentBooks = mapper.Map<List<MinimalBook>>(recentBooks);
            result.FavBooks = mapper.Map<List<MinimalBook>>(favoriteBooks);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete]
        [Route("profile/history")]
        public async Task<ActionResult> ClearHistory(
            [FromQuery] DateTime? mindate, 
            [FromQuery] DateTime? maxdate, 
            [FromQuery] bool lessons,
            [FromQuery] bool books)
        {
            var id = long.Parse(User.Claims.First().Value);
            var result = await repository.CleanHistory(id, lessons, books,
                mindate ?? DateTime.MinValue, 
                maxdate ?? DateTime.MaxValue
            );
            return Ok(new { changes = result });
        }

        [Authorize]
        [Route("profile/recents/courses")]
        public async Task<ActionResult> RecentCourses([FromQuery] int pag, [FromQuery] int lim)
        {
            var id = long.Parse(User.Claims.First().Value);
            var result = await repository.RecentsCourses(id, pag, lim);
            return Ok(result);
        }

        [Authorize]
        [Route("profile/recents/books")]
        public async Task<ActionResult> RecentBooks([FromQuery] int pag)
        {
            var id = long.Parse(User.Claims.First().Value);
            var result = await repository.RecentsBooks(id, pag);
            return Ok(result);
        }

        [Authorize]
        [Route("profile/favorite/courses")]
        public async Task<ActionResult> FavoriteCourses([FromQuery] int pag)
        {
            var id = long.Parse(User.Claims.First().Value);
            var result = await repository.FavoriteCourses(id, pag);
            return Ok(result);
        }

        [Authorize]
        [Route("profile/favorite/books")]
        public async Task<ActionResult> FavoriteBooks([FromQuery] int pag)
        {
            var id = long.Parse(User.Claims.First().Value);
            var result = await repository.FavoriteBooks(id, pag);
            return Ok(result);
        }

        [Authorize]
        [Route("profile/watchlater")]
        public async Task<ActionResult> ToWatchLater([FromQuery] int pag)
        {
            var id = long.Parse(User.Claims.First().Value);
            var data = await repository.LessonsToWatchLater(id, pag);
            var result = mapper.Map<List<MinimalLesson>>(data);
            return Ok(result);
        }
    }
}