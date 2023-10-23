using AutoMapper;
using MediatR;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Common.Repositories.UnitOfWork;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;

namespace VSATemplate.Features.Students.Commands
{
    public static class CreateFirstStudents
    {
        public class CreateFirstCommand : IRequest { }

        internal sealed class Handler: IRequestHandler<CreateFirstCommand>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOFWork)
            {
                _unitOfWork = unitOFWork;
            }

            public async Task Handle(CreateFirstCommand request, CancellationToken cancellationToken)
            {
                var firstStudent = new Student
                {
                    Name = "Sebastián Montemaggiore",
                    Email = "sebastianmontemaggiore@gmail.com",
                    Phone = "291544512"
                };
                await _unitOfWork.StudentRepository.Create(firstStudent);               

                var secondStudent = new Student
                {
                    Name = "Esteban Gonzalez",
                    Email = "estebangonzalez@gmail.com",
                    Phone = "291544536"
                };
                await _unitOfWork.StudentRepository.Create(secondStudent);                

                _ = await _unitOfWork.Commit();                
            }
        }

        public static void MapEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1.0/student/initialize", async (ISender sender) => 
            {
                var command = new CreateFirstCommand();

                await sender.Send(command);

                return Results.Ok();
            }).WithName("CreateFirstStudents");
        }
    }
}
