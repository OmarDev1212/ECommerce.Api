﻿using DomainLayer.Exceptions;
using Shared.Errors;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace ECommerce.Api.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                await HandlingNotFoundEndPoint(context);
            }
            catch (Exception ex)
            {

                //1.set status code
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    BadRequestException => StatusCodes.Status400BadRequest,
                    BadRequestWithErrorsException => StatusCodes.Status400BadRequest,
                    UnAuthorizedException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError,
                };
                //2. create custom response object
                var response = new ApiErrorResponse()
                {
                    StatusCode = context.Response.StatusCode,
                    ErrorMessage = ex.Message
                };
                // Handle special case for BadRequestWithErrorsException
                if (ex is BadRequestWithErrorsException badRequestWithErrors)
                {
                    response.Errors = badRequestWithErrors.Errors;
                }
                //3. change response type from string to json
                context.Response.ContentType = "application/json";


                //4.return object as json

                //await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
                //{
                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                //}));

                await context.Response.WriteAsJsonAsync(response);
            }
        }



        private static async Task HandlingNotFoundEndPoint(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var response = new ApiErrorResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"{context.Request.Path} Not found"
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}

