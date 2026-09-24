
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
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger,IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }
 
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var(statusCode, title) = MapException(exception);
            if (statusCode >= 500)
            {
                _logger.LogError(
                    exception,
                    "Unhandled server error occurred. Status: {StatusCode}, TraceId: {TraceId}",
                    statusCode,
                    httpContext.TraceIdentifier);
            }
            else
            {
                _logger.LogWarning("Handled client error: {Title}. Status: {StatusCode}, Detail: {Detail}, TraceId: {TraceId}",
                    title, statusCode, exception.Message, httpContext.TraceIdentifier);
            }
            string detail = (statusCode >= 500 && !_env.IsDevelopment())
                ? "An unexpected error occurred. Please contact support referencing the trace ID."
                : exception.Message;
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };
            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

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
