using AutoMapper;
using MediatR;
using VSATemplate.Features.Students.Common.Contracts;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;
using System.Data;
using FluentValidation;

namespace VSATemplate.Features.Students.Queries
{
    public static class GetStudent
    {
        public class GetQuery : IRequest<StudentGetDTO>
        {
            public Guid Id { get; set; }
        }

        public class Validator : AbstractValidator<string> 
        {
            public Validator()
            {
                RuleFor(Id => Id)
                    .Must(BeAValidGuid).WithMessage("The given Id is not of type Guid.");
                    //.Must(id => id != default(Guid)).WithMessage("El GUID no debe ser el valor por defecto.");
                // Guid x defecto ---> 00000000-0000-0000-0000-000000000000
            }

            private bool BeAValidGuid(string guid)
            {
                return Guid.TryParse(guid.ToString(), out _);
            }
        }

        internal sealed class Handler : IRequestHandler<GetQuery, StudentGetDTO>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOFWork, IMapper mapper)
            {
                _mapper = mapper;
                _unitOfWork = unitOFWork;
            }

            public async Task<StudentGetDTO> Handle(GetQuery request, CancellationToken cancellationToken)
            {
                var student = await _unitOfWork.StudentRepository.GetById(request.Id);

                if (student != null)
                {
                    return _mapper.Map<StudentGetDTO>(student);
                }
                else return null;
            }
        }

        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1.0/student/{id}", async (string id, ISender sender, IValidator<string> validator) =>
            {                
                var validationResult = validator.Validate(id);

                if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());
                else
                {
                    var query = new GetQuery { Id = Guid.Parse(id) };
                
                    var studentGetDTO = await sender.Send(query);

                    if (studentGetDTO == null) return Results.NotFound();
                    else return Results.Ok(studentGetDTO);
                }
            }).WithName("GetStudentById");
        }
    }
}
