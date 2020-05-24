using Atenz.Domain.DTOs;
using Atenz.Domain.Entities;
using AutoMapper;

namespace Atenz.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Lesson, LessonDTO>()
                .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.Module.Name));

            CreateMap<Lesson, ModuleLessonDTO>()
                .ForMember(dest => dest.WasWatched, opt => opt.MapFrom(src => (src.Watches.Count > 0)));
        }
    }
}