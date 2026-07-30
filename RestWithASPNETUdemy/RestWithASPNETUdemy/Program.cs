using Microsoft.EntityFrameworkCore;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Business.Implementations;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Repository;
using RestWithASPNETUdemy.Repository.Implementations;
using Microsoft.Data.SqlClient;
using EvolveDb;
using Serilog;
using RestWithASPNETUdemy.Repository.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Isso é para configurar a conexão com o banco de dados SQLServer usando o Entity Framework
var connectionString = builder.Configuration["SQLServerConnection:Connection"];
builder.Services.AddDbContext<SQLServerContext>(options => options.UseSqlServer(connectionString));

if (builder.Environment.IsDevelopment())
{
	MigrationDatabase(connectionString);
}

builder.Services.AddApiVersioning();

// Register services for Dependency Injection
builder.Services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IBookRepository, BookRepositoryImplementation>();
builder.Services.AddScoped<IBookBusiness, BookBusinessImplementation>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

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

void MigrationDatabase(string connection)
{
	try
	{
		var evolveConnection = new SqlConnection(connection);
		var evolve = new Evolve(evolveConnection, msg => Log.Information(msg))
		{
			Locations = new List<string> { "db/migrations", "db/dataset" },
			IsEraseDisabled = true
		};
		evolve.Migrate();
	}
	catch (Exception ex)
	{
		Log.Error("Database migration failed: {Error}", ex.Message);
		throw;
	}
}
