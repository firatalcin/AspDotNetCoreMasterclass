using Routing.CustomConstraints;

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

//5- Route Constraints Part 1

app.Map("products/details/{id:int?}", async context =>
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

app.Map("daily-digest-report/{reportdate:datetime}", async (context) => 
{
    DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);

    await context.Response.WriteAsync($"In daily-digest-report - {reportDate.ToShortDateString()}");
});


//5- Route Constraints Part 2

app.Map("cities/{cityid:guid}", async (context) => 
{
    Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!);
    await context.Response.WriteAsync($"City information - {cityId}");
});

//6- Route Constraints Part 3

app.Map("sales-report/{year:int:min(1900)}/{month:regex(^(apr|jul|oct|jan)$)}", async context =>
{
    int year = Convert.ToInt32(context.Request.RouteValues["year"]);
    string? month = Convert.ToString(context.Request.RouteValues["month"]);

    if (month == "apr" || month == "jul" || month == "oct" || month == "jan")
    {
        await context.Response.WriteAsync($"sales report - {year} - {month}");
    }
    else
    {
        await context.Response.WriteAsync($"{month} month is not allowed for sales report");
    }
});

//7- Custom Route Constraint Class


builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint));
});

app.Map("sales-report/{year:int:min(1900)}/{month:months}", async context =>
{
    int year = Convert.ToInt32(context.Request.RouteValues["year"]);
    string? month = Convert.ToString(context.Request.RouteValues["month"]);

    await context.Response.WriteAsync($"sales report - {year} - {month}");
});

app.Run();