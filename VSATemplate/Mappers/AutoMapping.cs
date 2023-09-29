using AutoMapper;
using VSATemplate.Entities;
using static VSATemplate.Features.Students.CreateStudent;

namespace VSATemplate.Mappers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Command, Student>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.DeletedTimeUtc, opt => opt.MapFrom(src => DateTime.MinValue)); // ¿Es correcto?
        }
    }
}
