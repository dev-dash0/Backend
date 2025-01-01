public class CustomUnauthorizedResponseMiddleware
{
    private readonly RequestDelegate _next;

    public CustomUnauthorizedResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(new
            {
                status = 401,
                title = "Unauthorized",
                detail = "You are not authorized to access this resource."
            }.ToString());
        }
    }
}

public static class CustomUnauthorizedResponseMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomUnauthorizedResponse(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomUnauthorizedResponseMiddleware>();
    }
}
