using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json;
using WebApi.Middlewares;

namespace WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }

        public static void AddAuthenticationExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["Jwt:Authority"];
                    options.Audience = configuration["Jwt:Audience"];
                    options.RequireHttpsMetadata = false;

                    // Captura errores y devuelve 401 en lugar de 404
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            var errorResponse = new { error = "No autorizado" };
                            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                        }
                    };
                });

            services.AddAuthorization();
        }

        public static void AddCorsExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var corsSettings = configuration["CORS:HostsPermitidos"];
            if (corsSettings == null) return;

            if (corsSettings == null) return;

            var origins = corsSettings.Split(';', StringSplitOptions.RemoveEmptyEntries);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                            //.AllowAnyOrigin() 
                            .WithOrigins(origins)  // Usa WithOrigins para utilizar cookies
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();  // Requiere WithOrigins
                });
            });

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll",
            //    builder =>
            //    {
            //        //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost" || new Uri(origin).Host == "myweb.local");
            //        builder.AllowAnyOrigin()
            //               .AllowAnyHeader()
            //               .AllowAnyMethod()
            //               .AllowCredentials()
            //               //.SetIsOriginAllowed(origin => true) // allow any origin
            //                .WithOrigins(origins.ToArray())
            //               ;
            //    });
            //});
        }

        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandleMiddleware>();
        }

        public static void UseLoggingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<LoggingMiddleware>();
        }
    }
}