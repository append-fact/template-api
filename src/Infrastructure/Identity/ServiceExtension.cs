using Application.Common.Interfaces;
using Application.Common.Wrappers;
using Domain.Settings;
using Identity.Context;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity
{

    public static class ServiceExtension
    {
        public static void AddIdentityInfraestructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

            //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
            services.AddIdentityCore<ApplicationUser>(opts =>
            {
                
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager();

            services
                .AddTransient<IAccountService, AccountService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IRolesService, RolesService>();


            #region Services
            services.AddTransient<IAccountService, AccountService>();
            #endregion

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]!)),
                    // Estos dos aseguran que:
                    // User.Identity.Name  ← ClaimTypes.Name 
                    // User.FindFirst(NameIdentifier) ← ClaimTypes.NameIdentifier (sub)
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role,
                };

                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "application/json";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();          // detiene la respuesta por defecto
                        context.Response.Clear();          // limpia headers ya escritos

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized; 
                        context.Response.ContentType = "application/json";
                        var payload = new Response<string>("Usted no está autorizado");
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(payload));
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject(new Response<string>("Usted no tiene permisos sobre este recurso"))
                        );
                    }
                };
            });





        }
    }
}
