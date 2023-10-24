using AutoMapper;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Students.Common.Contracts;
using static VSATemplate.Features.Students.Commands.Create;
using static VSATemplate.Features.Students.Commands.Update;

namespace VSATemplate.Features.Common.Mappers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CreateCommand, Student>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.DeletedTimeUtc, opt => opt.MapFrom(src => DateTime.MinValue));             

            CreateMap<UpdateCommand, Student>()                
                .ForMember(dest => dest.DeletedTimeUtc, opt => opt.MapFrom(src => DateTime.UtcNow)); // ¿Es correcto?

            CreateMap<Student, GetDTO>();
        }
    }
}
