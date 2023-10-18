using AutoMapper;
using MediatR;
using VSATemplate.Features.Students.Common.Contracts;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;

namespace VSATemplate.Features.Students.Queries
{
    public static class GetStudent
    {
        public class Query : IRequest<StudentGetDTO>
        {
            public Guid Id { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Query, StudentGetDTO>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOFWork, IMapper mapper)
            {
                _mapper = mapper;
                _unitOfWork = unitOFWork;
            }

            public async Task<StudentGetDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var student = await _unitOfWork.StudentRepository.GetById(request.Id);

                if (student != null)
                {
                    return _mapper.Map<StudentGetDTO>(student);
                }
                else return null;
            }
        }

        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1.0/student/{id}", async (Guid id, ISender sender) =>
            {
                var query = new Query { Id = id };

                var studentGetDTO = await sender.Send(query);

                if (studentGetDTO == null) return Results.NotFound();
                else return Results.Ok(studentGetDTO);
            }).WithName("GetStudentById");
        }
    }
}
