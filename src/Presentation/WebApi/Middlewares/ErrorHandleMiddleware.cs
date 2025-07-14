using Application.Common.Exceptions;
using Application.Common.Wrappers;
using System.Net;
using System.Text.Json;

namespace WebApi.Middlewares
{
    public class ErrorHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandleMiddleware> _logger;

        public ErrorHandleMiddleware(RequestDelegate next, ILogger<ErrorHandleMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                //Aca en el flujo si no hay error continua
                await _next(context);
            }
            catch (Exception error)
            {
                //Si hay error interceptamos la respuesta y escribimos en el log
                _logger.LogError(error, "An unhandled exception has occurred");

                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };

                response.StatusCode = error switch
                {
                    ApiException e => e.StatusCode, 
                    ValidationException => (int)HttpStatusCode.BadRequest,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                if (error is ValidationException validationError)
                {
                    responseModel.Errors = validationError.Errors;
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var result = JsonSerializer.Serialize(responseModel, options);

                await response.WriteAsync(result);
            }
        }
    }
}
