
using System.Net;
using ClinicManagementSystem.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.WebAPI.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
 
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var(statusCode, title) = MapException(exception);
            _logger.LogError(exception, "Handled exception with status code {StatusCode}", statusCode);
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message
            };
            if (exception is ValidationException validationException)
            {
                var errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                problemDetails.Extensions["errors"] = errors;
            }
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
        private static (int StatusCode, string Title) MapException(Exception exception)
        {
            return exception switch
            {
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, "Resource not found"),
                ValidationException => ((int)HttpStatusCode.BadRequest, "Validation failed"),
                InvalidOperationException => ((int)HttpStatusCode.BadRequest, "Invalid operation"),
                UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Unauthorized"),
                ForbiddenAccessException => ((int)HttpStatusCode.Forbidden, "Forbidden"),
                _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred")
            };
        }

    }
}
