using AutoMapper;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Students.Common.Contracts;
using static VSATemplate.Features.Students.Commands.CreateStudent;

namespace VSATemplate.Features.Common.Mappers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Command, Student>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.DeletedTimeUtc, opt => opt.MapFrom(src => DateTime.MinValue)); // ¿Es correcto?

            CreateMap<Student, StudentGetDTO>();
        }
    }
}
