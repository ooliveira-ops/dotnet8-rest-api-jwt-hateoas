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
using RestWithASPNETUdemy.Hypermedia.Filters;
using RestWithASPNETUdemy.Hypermedia.Enricher;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Cors configuration
builder.Services.AddCors(Options =>
{
	Options.AddDefaultPolicy(builder =>
	{
		builder.AllowAnyOrigin()
			   .AllowAnyMethod()
			   .AllowAnyHeader();
	});
});

// Add services to the container.
builder.Services.AddControllers();
//Custom Serialization
builder.Services.AddControllers(options =>
{
	options.RespectBrowserAcceptHeader = true; // Respeita o cabeçalho Accept do navegador/cliente
	options.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml"); // Mapeia o formato xml
	options.FormatterMappings.SetMediaTypeMappingForFormat("json", "application/json"); // Mapeia o formato json
	options.ReturnHttpNotAcceptable = true;   // Retorna 406 Not Acceptable se o formato não for suportado
}).AddXmlSerializerFormatters();

var filterOptions = new HyperMediaFilterOptions();
filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
filterOptions.ContentResponseEnricherList.Add(new BookEnricher());
builder.Services.AddSingleton(filterOptions);

//Isso é para configurar a conexão com o banco de dados SQLServer usando o Entity Framework
var connectionString = builder.Configuration["SQLServerConnection:Connection"];
builder.Services.AddDbContext<SQLServerContext>(options => options.UseSqlServer(connectionString));

if (builder.Environment.IsDevelopment())
{
	MigrationDatabase(connectionString);
}

builder.Services.AddApiVersioning();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Title = "Rest API's From 0 to Azure this ASP.NET Core 8 and Docker",
		Version = "v1",
		Description = "API RESTful developed in course 'Rest API's From 0 to Azure this ASP.NET Core 8 and Docker'",
		Contact = new Microsoft.OpenApi.Models.OpenApiContact
		{
			Name = "Filipe Oliveira",
			Url = new Uri("https://github.com/ooliveira-ops")
		}
	});
});

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

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI(c => {
	c.SwaggerEndpoint("/swagger/v1/swagger.json",
	"RestWithASPNETUdemy .NET 8 v1");
});

var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);

app.MapControllers();
app.MapControllerRoute("DefaultApi", "{controller=values}/v{version=apiVersion}/{id?}");

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
