public static class UserEndpoints
{
    public static void MapUserEndpoints(WebApplication app)
    {
        app.MapPost("/login", () => Results.Ok(new { action = "login" }));
        app.MapPost("/register", () => Results.Ok(new { action = "register" }));
        app.MapGet("/users", () => Results.Ok(new { action = "list" }));
        app.MapGet("/user/{id}", (int id) => Results.Ok(new { action = "get", userId = id }));
        app.MapPut("/user/{id}", (int id) => Results.Ok(new { action = "update", userId = id }));
        app.MapDelete("/user/{id}", (int id) => Results.Ok(new { action = "delete", userId = id }));
    }
}
