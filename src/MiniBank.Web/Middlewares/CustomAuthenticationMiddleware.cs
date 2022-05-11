using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MiniBank.Web.Exceptions;

namespace MiniBank.Web.Middlewares;

public class CustomAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public CustomAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            var token = httpContext.Request.Headers.Authorization.ToString();

            if (token.Length == 0)
            {
                throw new NotAuthorizedException("You are not authorized!");
            }

            var indexOfFirstDot = token.IndexOf('.');
            var indexOfSecondDot = token[(indexOfFirstDot + 1)..token.Length].IndexOf('.');

            var data = System.Text.Encoding.UTF8.GetString(
                Convert.FromBase64String(token[(indexOfFirstDot + 1)..(indexOfSecondDot - 1)]));

            var indexOfExpirationDate = data.IndexOf("exp", StringComparison.Ordinal);
            var indexOfNextComma = data[(indexOfExpirationDate + 1)..data.Length].IndexOf(',');
            
            var expirationDateInUnixTime = Convert.ToInt32(data[(indexOfExpirationDate + 5)..(indexOfExpirationDate + indexOfNextComma + 1)]);

            var expirationDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            expirationDate = expirationDate.AddSeconds(expirationDateInUnixTime);

            if (expirationDate < DateTime.UtcNow)
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await httpContext.Response.WriteAsJsonAsync("Token is expired");
            }
            else
            {
                await _next(httpContext);
            }
        }
        catch (Exception exception)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new { Error = $"{exception.Message}" });
        }
    }
}