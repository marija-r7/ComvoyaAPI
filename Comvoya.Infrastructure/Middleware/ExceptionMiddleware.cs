using Comvoya.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Comvoya.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ProblemDetails problem;

            switch (exception)
            {
                case UserLoginFailedException:
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Title = "Invalid Credentials",
                        Detail = exception.Message
                    };
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    break;

                case UserNotFoundException:
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "User Not Found",
                        Detail = exception.Message
                    };
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;

                case TripNotFoundException:
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Trip Not Found",
                        Detail = exception.Message
                    };
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;

                case InterestNotFoundException:
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Interest Not Found",
                        Detail = exception.Message
                    };
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;

                case UserInterestNotFoundException:
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "User Interest Not Found",
                        Detail = exception.Message
                    };
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;

                case UsernameAlreadyExistsException:
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Username Conflict",
                        Detail = exception.Message
                    };
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    break;

                case InterestAlreadyExistsException:
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Interest Already Exists",
                        Detail = exception.Message
                    };
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    break;

                case TripOnThisDateAlreadyExistsException:
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Trip Already Exists For This Date",
                        Detail = exception.Message
                    };
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    break;

                case TripCapacityFullException:
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Trip Capacity Full",
                        Detail = exception.Message
                    };
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    break;

                default:
                    problem = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Internal Server Error",
                        Detail = exception.Message
                    };
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(problem);
            return context.Response.WriteAsync(json);
        }
    }
}
