using AutoMapper;
using FluentValidation;
using MediatR;
using System.Security.Cryptography.X509Certificates;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;

namespace VSATemplate.Features.Students.Commands
{
    public static class SoftDeleteStudent
    {
        public class SoftDeleteCommand : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Validator : AbstractValidator<SoftDeleteCommand> 
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().NotNull();
            }
        }

        internal sealed class Handler : IRequestHandler<SoftDeleteCommand>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task Handle(SoftDeleteCommand command, CancellationToken cancellationToken)             
            {
                await _unitOfWork.StudentRepository.SoftDelete(command.Id);

                await _unitOfWork.Commit();
            }          
        }

        public static void MapEndpoint(IEndpointRouteBuilder app) 
        {
            app.MapDelete("api/v1.0/student/soft/{id}", (Guid id, ISender sender, IValidator<SoftDeleteCommand> validator) => 
            {
                var command = new SoftDeleteCommand { Id = id };  

                var validationResult = validator.Validate(command);

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                _ = sender.Send(command);

                return Results.Ok();
                
            }).WithName("SoftDeleteStudentById");
        }
        
    }
}
