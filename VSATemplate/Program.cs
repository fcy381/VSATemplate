using Microsoft.EntityFrameworkCore;
using VSATemplate.Data;
using VSATemplate.Repositories.UnitOfWork.Base;
using VSATemplate.Repositories.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("VSATemplate"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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

app.UseHttpsRedirection();

app.Run();

