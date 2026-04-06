var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//1- Map, MapGet, MapPost

app.MapGet("map1", async (context) => {
    await context.Response.WriteAsync("In Map 1");
});

app.MapPost("map2", async (context) => {
    await context.Response.WriteAsync("In Map 2");
});

app.MapFallback(async context => {
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

//2- Route Parameters

app.Map("files/{filename}.{extension}", async context =>
{
    string? fileName = Convert.ToString(context.Request.RouteValues["filename"]);
    string? extension = Convert.ToString(context.Request.RouteValues["extension"]);

    await context.Response.WriteAsync($"In files - {fileName} - {extension}");
});

app.Map("employee/profile/{EmployeeName}", async context => 
{
    string? employeeName = Convert.ToString(context.Request.RouteValues["employeename"]);
    await context.Response.WriteAsync($"In Employee profile - {employeeName}");
});

app.MapFallback(async (context) =>
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

app.Run();