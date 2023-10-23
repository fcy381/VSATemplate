using Microsoft.EntityFrameworkCore;
using VSATemplate.Features.Students.Commands;
using VSATemplate.Features.Students.Queries;
using VSATemplate.Features.Common.Repositories.UnitOfWork;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;
using VSATemplate.Features.Students.Common.Repository;
using VSATemplate.Features.Students.Common.Repository.Base;
using VSATemplate.Features.Common.Data;
using VSATemplate.Features.Courses.Common.Repository.Base;
using VSATemplate.Features.Courses.Common.Repository;
using VSATemplate.Features.Teachers.Common.Repository.Base;
using VSATemplate.Features.Teachers.Common.Repository;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("VSATemplate"));

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped<IValidator<CreateStudent.CreateCommand>, CreateStudent.Validator>();
builder.Services.AddScoped<IValidator<HardDeleteStudent.HardDeleteCommand>,HardDeleteStudent.Validator>();
builder.Services.AddScoped<IValidator<SoftDeleteStudent.SoftDeleteCommand>,SoftDeleteStudent.Validator>();
builder.Services.AddScoped<IValidator<UpdateStudent.UpdateCommand>,UpdateStudent.Validator>();
builder.Services.AddScoped<IValidator<string>,GetStudent.Validator>();

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

CreateStudent.MapEndpoint(app);
GetStudent.MapEndpoint(app);
GetAllStudents.MapEnpoint(app);
CreateFirstStudents.MapEndpoint(app);
HardDeleteStudent.MapEnpoint(app);
SoftDeleteStudent.MapEndpoint(app);
UpdateStudent.MapEndpoint(app);

app.UseHttpsRedirection();

app.Run();

