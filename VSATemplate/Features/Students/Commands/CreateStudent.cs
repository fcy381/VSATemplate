using MediatR;
using AutoMapper;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Students.Common.Contracts;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;

namespace VSATemplate.Features.Students.Commands
{
    public static class CreateStudent
    {
        public class Command : StudentPostDTO, IRequest<Student> { }

        internal sealed class Handler : IRequestHandler<Command, Student>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOFWork, IMapper mapper)
            {
                _mapper = mapper;
                _unitOfWork = unitOFWork;
            }

            public async Task<Student> Handle(Command request, CancellationToken cancellationToken)
            {
                var student = _mapper.Map<Student>(request);

                await _unitOfWork.StudentRepository.Create(student);
                _ = await _unitOfWork.Commit();

                return student;
            }
        }

        public static void MapEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1.0/student/", async (Command command, ISender sender, IMapper mapper) =>
            {
                var student = await sender.Send(command);

                var studentCreated = mapper.Map<StudentGetDTO>(student);

                return Results.CreatedAtRoute("GetStudentById", new { id = student.Id }, studentCreated);                
            }).WithName("CreateStudent");
        }
    }
}
