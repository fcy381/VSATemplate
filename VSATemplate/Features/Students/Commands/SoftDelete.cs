using AutoMapper;
using FluentValidation;
using MediatR;
using System.Security.Cryptography.X509Certificates;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;

namespace VSATemplate.Features.Students.Commands
{
    public static class SoftDelete
    {
        public class SoftDeleteCommand : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Validator : AbstractValidator<string> 
        {
            public Validator()
            {
                RuleFor(Id => Id)
                    .NotEmpty()
                    .NotNull()
                    .Must(BeAValidGuid).WithMessage("The given Id is not of type Guid.");
            }

            private bool BeAValidGuid(string guid)
            {
                return Guid.TryParse(guid.ToString(), out _);
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
            app.MapDelete("api/v1.0/student/soft/{id}", (string id, ISender sender, IValidator<string> validator) => 
            {
                var validationResult = validator.Validate(id);

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());
                else 
                { 
                    var command = new SoftDeleteCommand { Id = Guid.Parse(id) };

                    _ = sender.Send(command);

                    return Results.Ok();
                }
            }).WithName("SoftDeleteStudentById");
        }
        
    }
}
