using Microsoft.AspNetCore.Builder;
using RestWithASPNETUdemy.Services;
using RestWithASPNETUdemy.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register services for Dependency Injection
builder.Services.AddScoped<IPersonService, PersonServiceImplementation>();

// Add logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();


