using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;
using VSATemplate.Features.Common.Repositories.UnitOfWork;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;

namespace VSATemplate.Features.Students.Commands
{
    public static class HardDeleteStudent
    {
        public class HardDeleteCommand : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Validator : AbstractValidator<HardDeleteCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().NotNull();
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
            app.MapDelete("api/v1.0/student/hard/{id}", (Guid id, ISender sender, IValidator<HardDeleteCommand> validator) =>
            {
                var command = new HardDeleteCommand { Id = id };

                var validationResult = validator.Validate(command);

                if (!validationResult.IsValid)                
                    return Results.ValidationProblem(validationResult.ToDictionary());                

                _ = sender.Send(command);
                
                return Results.Ok();
            }).WithName("HardDeleteStudentById");
        
        }

    }
}
