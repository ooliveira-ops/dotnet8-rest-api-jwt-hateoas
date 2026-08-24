using System.Text;
using EvolveDb;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Business.Implementations;
using RestWithASPNETUdemy.Configurations;
using RestWithASPNETUdemy.Hypermedia.Enricher;
using RestWithASPNETUdemy.Hypermedia.Filters;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Repository;
using RestWithASPNETUdemy.Repository.Generic;
using RestWithASPNETUdemy.Repository.Implementations;
using RestWithASPNETUdemy.Services;
using RestWithASPNETUdemy.Services.Implementations;
using Serilog;

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
builder.Services.AddDbContext<SQLServerContext>(options => options.UseSqlServer(connectionString)
 .EnableSensitiveDataLogging());

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
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped<ILoginBusiness, LoginBusinessImplementation>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


// Configure JWT Authentication
var tokenConfigurations = new TokenConfiguration(); // -> Cria o objeto vazio
new ConfigureFromConfigurationOptions<TokenConfiguration>(
	builder.Configuration.GetSection("TokenConfiguration")) // -> Pega a seção do appsettings.json e configura o objeto vazio
	.Configure(tokenConfigurations);
builder.Services.AddSingleton(tokenConfigurations); // -> Registra ele na DI

builder.Services.AddAuthentication(options => // -> define QUAL esquema de autenticação usar (JWT Bearer)
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => // -> usa tokenConfigurations pra validar o token
{
	var paramsValidation = options.TokenValidationParameters;
	paramsValidation.ValidateIssuer = true;
	paramsValidation.ValidateAudience = true;
	paramsValidation.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret));
	paramsValidation.ValidAudience = tokenConfigurations.Audience;
	paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
	paramsValidation.ValidateIssuerSigningKey = true;
	paramsValidation.ValidateLifetime = true;
	paramsValidation.ClockSkew = TimeSpan.Zero;
});

// Defines who is allowed to access an endpoint after it is already authenticated.
builder.Services.AddAuthorization(auth =>
{
// Cria a política de autorização "Bearer": exige que o usuário
// esteja autenticado via JWT (token válido) para acessar o recurso

	auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
		.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
		.RequireAuthenticatedUser().Build());
});


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

app.UseAuthentication();
app.UseAuthorization();


// Enable Swagger middleware
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

// Here is the method to perform database migration using Evolve
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
