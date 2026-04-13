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


//3- Default Parameters

app.Map("employee/profile/{EmployeeName=scott}", async context =>
{
    string? employeeName = Convert.ToString(context.Request.RouteValues["employeename"]);
    await context.Response.WriteAsync($"In Employee profile - {employeeName}");
});

app.Map("products/details/{id=1}", async context =>
{
    int id = Convert.ToInt32(context.Request.RouteValues["id"]);
    await context.Response.WriteAsync($"Product details - {id}");
});

//4- Optional Parameters

app.Map("products/details/{id?}", async context =>
{
    if (context.Request.RouteValues.ContainsKey("id"))
    {
        int id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product details - {id}");
    }
    else
    {
        await context.Response.WriteAsync($"Product details - id is not supplied");
    }
});

app.Run();