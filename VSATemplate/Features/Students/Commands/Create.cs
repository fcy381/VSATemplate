using MediatR;
using AutoMapper;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Students.Common.Contracts;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Carter;
using static VSATemplate.Features.Students.Commands.Create;

namespace VSATemplate.Features.Students.Commands
{
    public static class Create
    {        

        public class CreateCommand : PostDTO, IRequest<IResult> { }

        public class Validator: AbstractValidator<CreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(60);
                RuleFor(x => x.Email).NotEmpty().MaximumLength(60);
                RuleFor(x => x.Phone).NotEmpty().MaximumLength(12);
            }
        }

        internal sealed class Handler : IRequestHandler<CreateCommand, IResult>
        {            
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOFWork, IMapper mapper)
            {
                _mapper = mapper;
                _unitOfWork = unitOFWork;
            }

            public async Task<IResult> Handle(CreateCommand request, CancellationToken cancellationToken)
            {
                var student = _mapper.Map<Student>(request);

                await _unitOfWork.StudentRepository.Create(student);
                
                await _unitOfWork.Commit();

                var studentCreated = _mapper.Map<GetDTO>(student);

                return Results.CreatedAtRoute("GetStudentById", new { id = student.Id }, studentCreated);                 
            }
        }

        //public static void MapEndpoint(this IEndpointRouteBuilder app)
        //{
        //    app.MapPost("/api/v1.0/student", async ([FromBody] CreateCommand createCommand, ISender sender, IValidator<CreateCommand> validator) =>
        //    {
        //        var validationResult = validator.Validate(createCommand);

        //        if (!validationResult.IsValid)                
        //            return Results.ValidationProblem(validationResult.ToDictionary());
                
        //        return await sender.Send(createCommand);                
        //    }).WithName("CreateStudent");
        //}
    }

    public class CreateStudentEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1.0/student", async ([FromBody] CreateCommand createCommand, ISender sender, IValidator<CreateCommand> validator) =>
            {
                var validationResult = validator.Validate(createCommand);

                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                return await sender.Send(createCommand);
            }).WithName("CreateStudent");
        }
    }
}
