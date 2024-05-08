using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

namespace API.Middlewares;

public static class ExceptionMiddlewareExtension
{
    public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(c => c.Run(async context =>
        {
            context.Response.StatusCode = 400;
            var exception = context.Features
                .Get<IExceptionHandlerPathFeature>()
                ?.Error;

            ErrorResult? error;
            if (exception is AppException appException)
            {
                error = (ErrorResult?)appException;
            }
            else
            {
                error = new ErrorResult
                {
                    Code = 500,
#if DEBUG
                    Message = exception.Message,
#else
                    Message = "خطای ناشناخته در سرویس",
#endif
                    Target = "Unhandled Exception"
                };
            }
            //logger.LogError($"Something went wrong: {exception}");
            await context.Response.WriteAsJsonAsync(error);
        }));
    }
    private struct ErrorResult
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Target { get; set; }

        public static explicit operator ErrorResult?(AppException? exception)
        {
            return exception is null ? null : new ErrorResult
            {
                Code = exception.Code,
                Message = exception.Message,
                Target = exception.Target
            };
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}