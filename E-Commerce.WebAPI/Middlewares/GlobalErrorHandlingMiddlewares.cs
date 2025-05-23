﻿using Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.ErroModels;

namespace E_Commerce.WebAPI.Middlewares
{
    public class GlobalErrorHandlingMiddlewares
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddlewares> _logger;
        public GlobalErrorHandlingMiddlewares(RequestDelegate next, ILogger<GlobalErrorHandlingMiddlewares> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await _next.Invoke(context);
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandlingNotFoundEndPointAsync(context);
                }
            }
            catch (Exception ex)
            {
                // log Exception 
                _logger.LogError(ex, ex.Message);
                await HandlingErrorAsync(context, ex);
            }

        }

        private static async Task HandlingErrorAsync(HttpContext context, Exception ex)
        {
            // 1. Set Status Code For Response 
            // 2. Set Content Type Code For Response 
            // 3. Response Object (Body)
            // Return Response 

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ErrorDetails()
            {
                ErrorMessage = ex.Message
            };

            response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ValidationException => HandlingValidationExceptionAsync((ValidationException)ex,response),
                _ => StatusCodes.Status500InternalServerError,
            };

            context.Response.StatusCode = response.StatusCode;

            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandlingNotFoundEndPointAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"End Point {context.Request.Path} is not Found"
            };
            await context.Response.WriteAsJsonAsync(response);
        }

        private static int HandlingValidationExceptionAsync(ValidationException ex , ErrorDetails response)
        {
            response.Errors = ex.Errors;
            return StatusCodes.Status400BadRequest;
        }


    }
}
