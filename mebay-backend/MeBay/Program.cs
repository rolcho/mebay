using MeBay.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddDbContext<MeBayDbContext>(options =>
{
    var connectionString = config["ConnectionStrings:DefaultConnection"];
    options.UseNpgsql(connectionString);
});

builder.Services.AddCors(
    options =>
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.AllowAnyOrigin();
        })
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MeBayDbContext>();
        dbContext.Database.Migrate();
    }
}

var port = config["PORT"];
builder.WebHost.UseUrls($"http://*:{port}");

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

UserEndpoints.MapUserEndpoints(app);

app.UseCors();

app.Run();

public partial class Program { }
