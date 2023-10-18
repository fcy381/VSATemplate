using MediatR;
using VSATemplate.Features.Common.Repositories.UnitOfWork;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;

namespace VSATemplate.Features.Students.Commands
{
    public static class HardDeleteStudent
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Command>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                await _unitOfWork.StudentRepository.HardDelete(request.Id);

                await _unitOfWork.Commit();
            }
        }

        public static void MapEnpoint(IEndpointRouteBuilder app) 
        {
            app.MapDelete("api/v1.0/student/hard/{id}", (Guid id, ISender sender) =>
            {
                var command = new Command { Id = id };

                _ = sender.Send(command);
                
                return Results.Ok();
            }).WithName("HardDeleteStudentById");
        
        }

    }
}
