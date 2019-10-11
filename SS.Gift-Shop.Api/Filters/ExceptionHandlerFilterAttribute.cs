using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SS.GiftShop.Core.Exceptions;

namespace SS.GiftShop.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ExceptionHandlerFilterAttribute : ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            await OnCustomException(context);

            if (context.Result != null)
            {
                // Custom exception handled by subclass
                return;
            }

            var exception = context.Exception;
            if (exception is EntityNotFoundException)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.NotFound);
                return;
            }

            if (exception is ObjectValidationException objectValidationException)
            {
                BadRequest(context, objectValidationException);
                return;
            }

            if (exception is BusinessLogicException)
            {
                context.ModelState.AddModelError("", exception.Message);
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }

            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<ExceptionHandlerFilterAttribute>>();

            logger.LogError(exception,
                $"Unhandled exception: {exception.Message}");

            InternalServerError(context, exception);
        }

        protected virtual Task OnCustomException(ExceptionContext context)
        {
            return Task.CompletedTask;
        }

        private static void InternalServerError(ExceptionContext context, Exception exception)
        {
            var environment = context.HttpContext.RequestServices
                .GetRequiredService<IHostingEnvironment>();
            if (environment.IsDevelopment())
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = new JsonResult(new
                {
                    error = new[] { exception.Message },
                    stackTrace = exception.StackTrace
                });
                return;
            }

            context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }

        private static void BadRequest(ExceptionContext context, ObjectValidationException exception)
        {
            foreach (var kvp in exception.Errors)
            {
                context.ModelState.AddModelError(kvp.Key, kvp.Value);
            }

            if (context.ModelState.Count == 0)
            {
                context.ModelState.AddModelError("", exception.Message);
            }

            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }
}
