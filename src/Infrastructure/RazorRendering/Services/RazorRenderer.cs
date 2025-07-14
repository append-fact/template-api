using Application.Common.Interfaces;
using RazorLight;

namespace RazorRendering.Services
{
    public class RazorRenderer : IRazorRenderer
    {
        private readonly RazorLightEngine _engine;

        public RazorRenderer()
        {
            // Aquí se configura el proyecto embebido indicando el namespace base de los templates
            _engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(RazorRenderer).Assembly, "RazorRendering.Resources.Templates")
                .UseMemoryCachingProvider()
                .Build();
        }

        // Método para renderizar la plantilla pasando el nombre de la plantilla y el modelo
        public async Task<string> RenderTemplateAsync(string templateName, object model)
        {
            return await _engine.CompileRenderAsync(templateName, model);
        }
    }
}

