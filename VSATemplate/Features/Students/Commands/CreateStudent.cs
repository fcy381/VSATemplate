using MediatR;
using AutoMapper;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Students.Common.Contracts;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;

namespace VSATemplate.Features.Students.Commands
{
    public static class CreateStudent
    {
        public class CreateCommand : StudentPostDTO, IRequest<IResult> { }

        internal sealed class Handler : IRequestHandler<CreateCommand, IResult>
        {            
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOFWork, IMapper mapper)
            {
                _mapper = mapper;
                _unitOfWork = unitOFWork;
            }

            public async Task<IResult> Handle(CreateCommand request, CancellationToken cancellationToken)
            {
                var student = _mapper.Map<Student>(request);

                await _unitOfWork.StudentRepository.Create(student);
                
                await _unitOfWork.Commit();

                var studentCreated = _mapper.Map<StudentGetDTO>(student);

                return Results.CreatedAtRoute("GetStudentById", new { id = student.Id }, studentCreated);
            }
        }

        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1.0/student/", async (CreateCommand createCommand, ISender sender) =>
            {
                return await sender.Send(createCommand);                
            }).WithName("CreateStudent");
        }
    }
}
