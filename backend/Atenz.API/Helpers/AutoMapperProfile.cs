using Atenz.Domain.DTOs;
using Atenz.Domain.Entities;
using Atenz.Domain.Identity;
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

            CreateMap<User, ProfileDTO>()
                .ForMember(dest => dest.RegisteredAt, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<Lesson, Latest>()
                .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.Module.Name))
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Module.Course.Name))
                .ForMember(dest => dest.Banner, opt => opt.MapFrom(src => src.Module.Course.Banner));

            CreateMap<Course, MinimalCourse>();

            CreateMap<Lesson, MinimalLesson>()
                .ForMember(dest => dest.Banner, opt => opt.MapFrom(src => src.Module.Course.Banner));
            
            CreateMap<Book, MinimalBook>();
        }
    }
}