namespace Middleware.CustomMiddleware;

public class HelloCustomMiddleware
{
    private readonly RequestDelegate _next;
    
    public HelloCustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Query.ContainsKey("firstname") && context.Request.Query.ContainsKey("lastname"))
        {
            string fullName = context.Request.Query["firstname"] + " " + context.Request.Query["lastname"];
            await context.Response.WriteAsync(fullName);
        }
        await _next(context);
        //after logic
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class HelloCustomMiddleExtensions
{
    public static IApplicationBuilder UseHelloCustomMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HelloCustomMiddleware>();
    }
}