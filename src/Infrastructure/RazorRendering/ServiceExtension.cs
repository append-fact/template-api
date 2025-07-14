using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RazorRendering.Services;

namespace RazorRendering
{
    public static class ServiceExtension
    {
        public static void AddRazorTemplateEngine(this IServiceCollection services)
        {
            services.AddSingleton<IRazorRenderer, RazorRenderer>();
        }
    }
}
