public class ErrorHandlerService
{
    private readonly RequestDelegate _next;

    public ErrorHandlerService(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadHttpRequestException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            Console.WriteLine(
                $"{DateTime.UtcNow}  {context.Connection.RemoteIpAddress}   An error occurred: {ex.Message}"
            );
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            Console.WriteLine(
                $"{DateTime.UtcNow}  {context.Connection.RemoteIpAddress}   An error occurred: {ex.Message}"
            );
        }
    }
}
