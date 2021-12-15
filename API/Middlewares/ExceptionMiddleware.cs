using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Utility.Exceptions;
using Utility.Models;

namespace API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Before user response 
                await HandleExceptionAsync(context,ex,_env);
            }
            // For request code before this line
            // Call the next delegate/middleware in the pipeline
            // For response code after this line
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment env)
        {
            const HttpStatusCode code = HttpStatusCode.InternalServerError;
            var errors = new ApiErrorResponse()
            {
                StatusCode = (int) code
            };

            if (_env.IsDevelopment())
            {
                errors.Details = exception.StackTrace;
            }
            else
            {
                errors.Details = exception.Message;
            }

            switch (exception)
            {
                case ApplicationValidationException e :
                    errors.Message = e.Message;
                    errors.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                    break;
                default:
                    errors.Message = "Something is wrong with our server";
                    break;
            }

            var result = JsonConvert.SerializeObject(errors);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errors.StatusCode;
            await context.Response.WriteAsync(result);
        }
    }
}

