using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;

namespace CMS.API.Middlewares
{
    public class JwtExpiryMiddleware
    {
        readonly RequestDelegate _next;
        public JwtExpiryMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);

                    if (jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Token has expired. LogIn again");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid token.");
                    return;
                }
            }

            await _next(context);
        }
    }
}
