using AutoMapper;
using MediatR;
using System.Security.Cryptography.X509Certificates;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;

namespace VSATemplate.Features.Students.Commands
{
    public static class SoftDeleteStudent
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

            public async Task Handle(Command command, CancellationToken cancellationToken)             
            {
                await _unitOfWork.StudentRepository.SoftDelete(command.Id);

                await _unitOfWork.Commit();
            }          
        }

        public static void MapEndpoint(IEndpointRouteBuilder app) 
        {
            app.MapDelete("api/v1.0/student/soft/{id}", (Guid id, ISender sender) => 
            {
                var command = new Command { Id = id };  

                _ = sender.Send(command);

                return Results.Ok();
                
            }).WithName("SoftDeleteStudentById");
        }
        
    }
}
