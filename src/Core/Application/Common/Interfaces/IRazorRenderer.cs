namespace Application.Common.Interfaces
{
    public interface IRazorRenderer
    {
        Task<string> RenderTemplateAsync(string templateName, object model);
    }
}
