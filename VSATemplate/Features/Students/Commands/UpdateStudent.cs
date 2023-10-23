﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Common.Repositories.UnitOfWork;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;
using VSATemplate.Features.Students.Common.Contracts;

namespace VSATemplate.Features.Students.Commands
{
    public static class UpdateStudent
    {
        public class UpdateCommand : StudentUpdateDTO, IRequest<IResult> { }

        public class Validator : AbstractValidator<UpdateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(10);
                RuleFor(x => x.Email).NotEmpty().MaximumLength(5);
                RuleFor(x => x.Phone).NotEmpty().MaximumLength(15);
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
                var student = await _unitOfWork.StudentRepository.GetById(request.Id);

                if (student != null)
                {
                    _ = _mapper.Map<UpdateCommand, Student>(request, student);

                    await _unitOfWork.Commit();

                    return Results.Ok();
                }
                else return Results.BadRequest();                
            }
        }

        public static void MapEndpoint(this IEndpointRouteBuilder app) 
        {
            app.MapPut("api/v1.0/student/{id}", async (Guid id,[FromBody] UpdateCommand UpdateCommand, ISender sender, IValidator<UpdateCommand> validator) =>
            {
                var validationResult = validator.Validate(UpdateCommand);

                if (!validationResult.IsValid)                
                    return Results.ValidationProblem(validationResult.ToDictionary());                

                if (id != UpdateCommand.Id)
                    return Results.BadRequest();

                return await sender.Send(UpdateCommand);
            }).WithName("UpdateStudent");
        }

    }
}
