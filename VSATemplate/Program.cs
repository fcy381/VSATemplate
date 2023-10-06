using Microsoft.EntityFrameworkCore;
using static VSATemplate.Features.Students.Commands.CreateStudent;
using VSATemplate.Features.Students.Commands;
using VSATemplate.Features.Students.Queries;
using VSATemplate.Features.Common.Repositories.UnitOfWork;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;
using VSATemplate.Features.Students.Common.Repository;
using VSATemplate.Features.Students.Common.Repository.Base;
using VSATemplate.Features.Common.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("VSATemplate"));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

CreateStudent.MapEndpoints(app);
GetStudent.MapEndpoints(app);

app.UseHttpsRedirection();

app.Run();

