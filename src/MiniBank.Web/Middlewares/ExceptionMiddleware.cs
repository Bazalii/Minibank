﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MiniBank.Core.Exceptions;
using MiniBank.Data.Exceptions;
using MiniBank.Web.Exceptions;

namespace MiniBank.Web.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException validationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Error = $"{validationException.Message}" });
            }
            catch (FluentValidation.ValidationException exception)
            {
                var errors = exception.Errors
                    .Select(error => $"{error.PropertyName}: {error.ErrorMessage}");

                var errorMessage = string.Join(Environment.NewLine, errors);

                await httpContext.Response.WriteAsJsonAsync(new { Error = $"{errorMessage}" });
            }
            catch (ObjectNotFoundException objectNotFoundException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                await httpContext.Response.WriteAsJsonAsync(new { Error = $"{objectNotFoundException.Message}" });
            }
            catch (NotAuthorizedException notAuthorizedException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(new { Error = $"{notAuthorizedException.Message}" });
            }
            catch (Exception)
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(new { Error = "Internal error!" });
            }
        }
    }
}