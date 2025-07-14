using Application;
using Identity;
using Identity.Context;
using Identity.Models;
using Identity.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Contexts;
using Persistence.Seeds;
using RazorRendering;
using Serilog;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;
using Shared;
using Shared.RealTimeCommunication;
using System.Reflection;
using System.Text.Json.Serialization;
using WebApi.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Configura Kestrel para escuchar en el puerto 8085
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://*:8087");
}

//Application Layer
builder.Services.AddApplicationLayer(builder.Configuration);

//Identity Layer
builder.Services.AddIdentityInfraestructureLayer(builder.Configuration);

//Persistence Layer
builder.Services.AddPersistenceLayer(builder.Configuration);
//Shared Layer
builder.Services.AddSharedLayer(builder.Configuration);

//razor
builder.Services.AddRazorTemplateEngine();

builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// CORS
builder.Services.AddCorsExtension(builder.Configuration);

//Agrego instancia para versionado
builder.Services.AddApiVersioningExtension();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TemplateAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
  });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var loggerConfiguration = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext();

// Si no es development agregamos el sink de insights
if (!builder.Environment.IsDevelopment())
{

    builder.Services.AddApplicationInsightsTelemetry(options =>
    {
        options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    });

    loggerConfiguration = loggerConfiguration.WriteTo.ApplicationInsights(
        builder.Configuration["ApplicationInsights:ConnectionString"],
        new TraceTelemetryConverter());
}

Log.Logger = loggerConfiguration.CreateLogger();


builder.Host.UseSerilog();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
//}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseWebSockets();

app.UseStaticFiles();



app.UseAuthentication();
app.UseAuthorization();
//Aca usamos el middleware de logging
app.UseLoggingMiddleware();
//Aca usamos el middleware de errores
app.UseErrorHandlingMiddleware();


app.MapHub<RealTimeHub>("/realtime");


app.MapControllers();

try
{
    Log.Information("Iniciando Web API");

    await CargarSeeds();

    Log.Information("Corriendo");
    //Log.Information("https://localhost:44361");

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");

    return;
}
finally
{
    Log.CloseAndFlush();
}

//app.Run();

async Task CargarSeeds()
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var identityContext = services.GetRequiredService<IdentityContext>();
    // Aplicar migraciones pendientes solo si existen para Identity
    if (identityContext.Database.GetPendingMigrations().Any())
    {
        identityContext.Database.Migrate();
    }

    await DefaultRoles.SeedAsync(userManager, roleManager, identityContext);
    await DefaultUsers.SeedAsync(userManager, roleManager, identityContext);
    await DefaultRolePermissions.SeedAsync(roleManager, identityContext);

    var context = services.GetRequiredService<ApplicationDbContext>();
    // Aplicar migraciones pendientes solo si existen para la aplicación
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }

    await CountrySeed.SeedCountryAsync(context);
    await CountrySeed.SeedProvincesAsync(context);
    await CountrySeed.SeedCityAsync(context);
}