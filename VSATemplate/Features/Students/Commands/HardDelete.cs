using Carter;
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
        public class HardDeleteCommand : IRequest<IResult>
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

            private bool BeAValidGuid(string? guid)
            {
                if (guid is null) return false;
                else return Guid.TryParse(guid.ToString(), out _);
            }
        }

        internal sealed class Handler : IRequestHandler<HardDeleteCommand, IResult>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IResult> Handle(HardDeleteCommand request, CancellationToken cancellationToken)
            {
                await _unitOfWork.StudentRepository.HardDelete(request.Id);

                await _unitOfWork.Commit();

                return Results.Ok();
            }
        }

        //public static void MapEnpoint(IEndpointRouteBuilder app) 
        //{
        //    app.MapDelete("api/v1.0/student/hard/{id}", async (string? id, ISender sender, IValidator<string> validator) =>
        //    {                
        //        var validationResult = validator.Validate(id);

        //        if (!validationResult.IsValid)
        //            return Results.ValidationProblem(validationResult.ToDictionary());
        //        else 
        //        {
        //            var command = new SoftDeleteCommand { Id = Guid.Parse(id) };

        //            return await sender.Send(command);                    
        //        }                
        //    }).WithName("HardDeleteStudentById");
        //}
    }

    public class HardDeleteStudentByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/v1.0/student/hard/{id}", async (string? id, ISender sender, IValidator<string> validator) =>
            {
                var validationResult = validator.Validate(id);

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());
                else
                {
                    var command = new SoftDeleteCommand { Id = Guid.Parse(id) };

                    return await sender.Send(command);
                }
            }).WithName("HardDeleteStudentById");
        }
    }
}
