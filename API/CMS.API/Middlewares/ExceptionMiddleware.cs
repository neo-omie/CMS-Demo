using CMS.Application.Exceptions;
using System.Net;

namespace CMS.API.Middlewares
{
    public class ExceptionMiddleware
    {
        readonly RequestDelegate _next;
        //readonly ILogger<ExceptionMiddleware> _logger;
        //create constructor
        public ExceptionMiddleware(RequestDelegate next/*, ILogger<ExceptionMiddleware> logger*/)
        {
            _next = next;
            //_logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "", null);
                await HandleExceptionsAsync(httpContext, ex);
            }
        }

        private static async Task<Task> HandleExceptionsAsync(HttpContext httpContext, Exception ex)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            switch (ex)
            {
                case UserNotFoundException NotFound:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case DocumentNotFoundException NotFound:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case PasswordRenewalException pswdRenew:
                    statusCode = HttpStatusCode.Gone;
                    break;

                //case BadRequestException BadReq:
                //    statusCode = HttpStatusCode.BadRequest;
                    //break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            };
            httpContext.Response.StatusCode = (int)statusCode;
            var response = new
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = ex.Message

            };
            return httpContext.Response.WriteAsJsonAsync(response);
        }
    }
}
