using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using To_Do.API.Controllers;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace To_Do.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }
        public static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new { message = "An unexpected error occurred. Please try again later." };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}