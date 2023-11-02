using AutoMapper;
using MediatR;
using VSATemplate.Features.Students.Common.Contracts;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;
using System.Data;
using FluentValidation;
using Carter;
using static VSATemplate.Features.Students.Queries.Get;

namespace VSATemplate.Features.Students.Queries
{
    public static class Get
    {
        public class GetQuery : IRequest<IResult>
        {
            public Guid? Id { get; set; }
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
                if (guid is null) return false;
                else return Guid.TryParse(guid.ToString(), out _);
            }
        }

        internal sealed class Handler : IRequestHandler<GetQuery, IResult>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOFWork, IMapper mapper)
            {
                _mapper = mapper;
                _unitOfWork = unitOFWork;
            }

            public async Task<IResult> Handle(GetQuery request, CancellationToken cancellationToken)
            {
                var student = await _unitOfWork.StudentRepository.GetById(request.Id);

                if (student != null)
                {
                    return Results.Ok(_mapper.Map<GetDTO>(student));
                }
                else return Results.NotFound();
            }
        }

        

        //public static void MapEndpoint(this IEndpointRouteBuilder app)
        //{
        //    app.MapGet("/api/v1.0/student/{id}", async (string? id, ISender sender, IValidator<string> validator) =>
        //    {                
        //        var validationResult = validator.Validate(id);

        //        if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());
        //        else
        //        {
        //            var query = new GetQuery { Id = Guid.Parse(id) };

        //            return await sender.Send(query);
        //        }
        //    }).WithName("GetStudentById");
        //}
    }
    public class GetStudentByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1.0/student/{id}", async (string? id, ISender sender, IValidator<string> validator) =>
            {
                var validationResult = validator.Validate(id);

                if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());
                else
                {
                    var query = new GetQuery { Id = Guid.Parse(id) };

                    return await sender.Send(query);
                }
            }).WithName("GetStudentById");
        }
    }
}
