using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace UserAccessManagement.API.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled)
            return;

        context.Result = new ObjectResult(new
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Message = "An internal server error occurred.",
            DetailedMessage = context.Exception.Message
        })
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            ContentTypes = { "application/json" }
        };

        context.ExceptionHandled = true;
    }
}
