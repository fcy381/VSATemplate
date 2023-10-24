using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;
using VSATemplate.Features.Common.Repositories.UnitOfWork;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static VSATemplate.Features.Students.Commands.SoftDelete;

namespace VSATemplate.Features.Students.Commands
{
    public static class HardDelete
    {
        public class HardDeleteCommand : IRequest
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

        internal sealed class Handler : IRequestHandler<HardDeleteCommand>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task Handle(HardDeleteCommand request, CancellationToken cancellationToken)
            {
                await _unitOfWork.StudentRepository.HardDelete(request.Id);

                await _unitOfWork.Commit();
            }
        }

        public static void MapEnpoint(IEndpointRouteBuilder app) 
        {
            app.MapDelete("api/v1.0/student/hard/{id}", (string id, ISender sender, IValidator<string> validator) =>
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
            }).WithName("HardDeleteStudentById");
        
        }

    }
}
