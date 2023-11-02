using AutoMapper;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Common.Repositories.UnitOfWork;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;
using VSATemplate.Features.Students.Common.Contracts;
using static VSATemplate.Features.Students.Commands.Update;

namespace VSATemplate.Features.Students.Commands
{
    public static class Update
    {
        public class UpdateCommand : UpdateDTO, IRequest<IResult> { }

        public class Validator : AbstractValidator<UpdateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty()
                    .NotNull()
                    .Must(BeAValidGuid).WithMessage("The given Id is not of type Guid.");                   
                RuleFor(x => x.Name).NotEmpty().MaximumLength(60);
                RuleFor(x => x.Email).NotEmpty().MaximumLength(60);
                RuleFor(x => x.Phone).NotEmpty().MaximumLength(12);
            }

            private bool BeAValidGuid(string? guid)
            {
                if (guid is null) return false;                
                else return Guid.TryParse(guid.ToString(), out _);
            }
        }

        internal sealed class Handler : IRequestHandler<UpdateCommand, IResult>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IMapper mapper, IUnitOfWork unitOfWork)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
            }

            public async Task<IResult> Handle(UpdateCommand request, CancellationToken cancellationToken)
            {                
                var student = await _unitOfWork.StudentRepository.GetById(Guid.Parse(request.Id));

                if (student != null)
                {
                    _ = _mapper.Map<UpdateCommand, Student>(request, student);

                    await _unitOfWork.Commit();

                    return Results.Ok();
                }
                else return Results.NotFound();                
            }
        }

        //public static void MapEndpoint(this IEndpointRouteBuilder app) 
        //{
        //    app.MapPut("api/v1.0/student", async ([FromBody] UpdateCommand UpdateCommand, ISender sender, IValidator<UpdateCommand> validator) =>
        //    {
        //        var validationResult = validator.Validate(UpdateCommand);

        //        if (!validationResult.IsValid)                
        //            return Results.ValidationProblem(validationResult.ToDictionary());                
                
        //        return await sender.Send(UpdateCommand);
        //    }).WithName("UpdateStudent");
        //}
    }

    public class UpdateStudentEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1.0/student", async ([FromBody] UpdateCommand UpdateCommand, ISender sender, IValidator<UpdateCommand> validator) =>
            {
                var validationResult = validator.Validate(UpdateCommand);

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                return await sender.Send(UpdateCommand);
            }).WithName("UpdateStudent");
        }
    }
}
