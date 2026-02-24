var builder = WebApplication.CreateBuilder(args);

//builder.Services
//builder.Configuration
//builder.Environment


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();