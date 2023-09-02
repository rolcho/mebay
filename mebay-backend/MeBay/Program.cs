var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

UserEndpoints.MapUserEndpoints(app);
app.Run();

public partial class Program { }
