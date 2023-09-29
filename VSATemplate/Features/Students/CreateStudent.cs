using MediatR;
using VSATemplate.Repositories.GenericRepository.Base;
using VSATemplate.Repositories.GenericRepository;
using VSATemplate.Repositories.UnitOfWork.Base;
using VSATemplate.Entities;
using VSATemplate.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using VSATemplate.Repositories.UnitOfWork;

namespace VSATemplate.Features.Students
{
    public static class CreateStudent
    {
        public class Command : IRequest<int>
        {
            public string Name { get; set; } = string.Empty;

            public string Email { get; set; } = string.Empty;

            public string Phone { get; set; } = string.Empty;
        }

        public interface IStudentRepository : IGenericRepository<Student> {}

        public class StudentRepository : GenericRepository<Student>, IStudentRepository
        {
            public StudentRepository(DataContext dataContext) : base(dataContext) {}
        }

        internal sealed class Handler : IRequestHandler<Command, int>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler( IUnitOfWork unitOFWork, IMapper mapper)
            {
                _mapper = mapper;
                _unitOfWork = unitOFWork;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var student = _mapper.Map<Student>(request);

                await _unitOfWork.StudentRepository.Create(student);
                _ = await _unitOfWork.Commit();

                var list = _unitOfWork.StudentRepository.GetAll();

                return student.Id;
            }
        }

        public static void MapEndpoints(this IEndpointRouteBuilder app) 
        {
            app.MapPost("/", async (Command command, ISender sender) => 
            {
                var studentId = await sender.Send(command);

                return Results.Ok(studentId);
            });
        }
    }
}
