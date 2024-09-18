using DailyPlanner.Domain.Enum;
using DailyPlanner.Domain.Result;
using ILogger = Serilog.ILogger;

namespace DailyPlanner.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.Error(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var result = CreateErrorResponse(exception);
    
        context.Response.ContentType = "application/json";
        if (result.ErrorCode != null) 
            context.Response.StatusCode = (int)result.ErrorCode;
        
        await context.Response.WriteAsJsonAsync(result);
    }

    private BaseResult CreateErrorResponse(Exception exception)
    {
        return exception switch
        {
            UnauthorizedAccessException _ => new BaseResult
            {
                ErrorMessage = exception.Message,
                ErrorCode = ErrorCodes.UserUnauthorizedAccess
            },
            _ => new BaseResult
            {
                ErrorMessage = "Internal server error, please contact support!",
                ErrorCode = ErrorCodes.InternalServerError
            },
        };
    }
}