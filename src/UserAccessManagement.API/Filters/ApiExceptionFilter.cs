using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using UserAccessManagement.API.ActionResults;
using UserAccessManagement.Infrastructure.Exceptions;

namespace UserAccessManagement.API.Filters;

public sealed class ApiExceptionFilter : IExceptionFilter
{
    private readonly IWebHostEnvironment _environment;

    public ApiExceptionFilter(IWebHostEnvironment environment)
    {
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled)
            return;

        var message = context.Exception is BusinessException ? context.Exception.Message : "An internal server error occurred.";
        var detailedMessage = _environment.IsDevelopment() ? context.Exception.Message : null;

        var responseObject = new ErrorResult(HttpStatusCode.InternalServerError, message, detailedMessage);

        context.Result = new ObjectResult(responseObject)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            ContentTypes = { "application/json" }
        };

        context.ExceptionHandled = true;
    }
}