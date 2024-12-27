using DGPCE.Sigemad.API.Middleware;
using DGPCE.Sigemad.Application;
using DGPCE.Sigemad.Identity;
using DGPCE.Sigemad.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using NetTopologySuite.IO.Converters;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new GeometryConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options =>
{
    options.EnableAnnotations();
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.ConfigureIdentityServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    );
});

// Configuracion para multilenguaje
builder.Services.AddLocalization(options => options.ResourcesPath = "");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "es" };
    options.SetDefaultCulture("es");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

/*
// Configura la política predeterminada para permitir a todos
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequiereAutorizacion", policy =>
        policy.RequireAuthenticatedUser()); // Requiere que el usuario esté autenticado

    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAssertion(_ => true) // Permitir a todos
        .Build();
});
*/

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Habilitar multilenguaje
app.UseRequestLocalization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();


app.UseMiddleware<AuthenticatedUserMiddleware>();

app.UseCors("CorsPolicy");

app.UseSerilogRequestLogging();

app.MapControllers();


app.Run();
