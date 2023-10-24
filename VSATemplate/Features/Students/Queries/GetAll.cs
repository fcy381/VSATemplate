using AutoMapper;
using MediatR;
using System.Runtime.CompilerServices;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;
using VSATemplate.Features.Students.Common.Contracts;

namespace VSATemplate.Features.Students.Queries
{
    public static class GetAll
    {
        public class GetAllQuery : IRequest<List<GetDTO>> {}

        internal sealed class Handler : IRequestHandler<GetAllQuery, List<GetDTO>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOFWork, IMapper mapper)
            {
                _mapper = mapper;
                _unitOfWork = unitOFWork;
            }

            public async Task<List<GetDTO>> Handle(GetAllQuery request, CancellationToken cancellationToken)
            {
                var listStudents = _unitOfWork.StudentRepository.GetAll();

                if (listStudents != null)
                {
                    var studentsListDTO = new List<GetDTO>();

                    foreach (var stdt in listStudents)
                    {
                        var studentGetDTO = new GetDTO();

                        studentGetDTO = _mapper.Map<GetDTO>(stdt);

                        studentsListDTO.Add(studentGetDTO);
                    }

                    return studentsListDTO;
                }
                else return null;
            }

        }

        public static void MapEnpoint(this IEndpointRouteBuilder app) 
        {
            app.MapGet("/api/v1.0/student/all", async (ISender sender) => 
            { 
                var query = new GetAllQuery();

                var studentListDTO = await sender.Send(query);

                if (studentListDTO == null) return Results.NoContent();
                else return Results.Ok(studentListDTO);           
            }).WithName("GetAllStudent"); 
        }
    }
}
           