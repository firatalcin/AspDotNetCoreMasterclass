var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.Run(async (HttpContext context) =>
{

    // Http Response
    /*
    context.Response.Headers["MyKey"] = "My Value";
    context.Response.Headers["Server"] = "My Server";
    context.Response.Headers["Content-Type"] = "text/html";
    context.Response.StatusCode = 400;
    */

    // Http Request
    string path = context.Request.Path;
    string method = context.Request.Method;
    context.Response.Headers["Content-Type"] = "text/html";
    await context.Response.WriteAsync($"<h1>{path}</h1>");
    await context.Response.WriteAsync($"<h1>{method}</h1>");
    
});

app.Run();
