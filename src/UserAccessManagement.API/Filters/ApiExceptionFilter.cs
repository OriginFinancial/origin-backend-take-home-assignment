using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using UserAccessManagement.API.ActionResults;

namespace UserAccessManagement.API.Filters;

public class ApiExceptionFilter : IExceptionFilter
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

        var detailedMessage = _environment.IsDevelopment() ? context.Exception.Message : null;
        var responseObject = new ErrorResult(StatusCode: HttpStatusCode.InternalServerError, Message: "An internal server error occurred.", DetailedMessage: detailedMessage);

        context.Result = new ObjectResult(responseObject)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            ContentTypes = { "application/json" }
        };

        context.ExceptionHandled = true;
    }
}