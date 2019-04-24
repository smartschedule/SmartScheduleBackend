namespace SmartSchedule.Api.Filters
{
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using SmartSchedule.Api.Models;
    using SmartSchedule.Application.Exceptions;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException ex)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(ex.Failures);
            }
            else if (context.Exception is FluentValidation.ValidationException)
            {
                var exception = context.Exception as FluentValidation.ValidationException;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(new ValidationResultModel(exception));
            }
            else
            {

                var code = HttpStatusCode.InternalServerError;

                if (context.Exception is NotFoundException)
                {
                    code = HttpStatusCode.NotFound;
                }

                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)code;
                context.Result = new JsonResult(new
                {
                    error = new[] { context.Exception.Message },
                    stackTrace = context.Exception.StackTrace
                });
            }

        }
    }
}
