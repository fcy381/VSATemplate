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

builder.Services.AddScoped<IValidator<Create.CreateCommand>, Create.Validator>();
builder.Services.AddScoped<IValidator<string>,HardDelete.Validator>();
builder.Services.AddScoped<IValidator<string>,SoftDelete.Validator>();
builder.Services.AddScoped<IValidator<Update.UpdateCommand>,Update.Validator>();
builder.Services.AddScoped<IValidator<string>,Get.Validator>();

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

Create.MapEndpoint(app);
Get.MapEndpoint(app);
GetAll.MapEnpoint(app);
CreateFirsts.MapEndpoint(app);
HardDelete.MapEnpoint(app);
SoftDelete.MapEndpoint(app);
Update.MapEndpoint(app);

app.UseHttpsRedirection();

app.Run();

