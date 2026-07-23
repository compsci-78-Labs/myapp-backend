namespace WebApi.Extensions;

public static class MiddleWareExtensions
{
    public static WebApplication UseApplicationMiddleware(
        this WebApplication app)
    {
        app.UseHttpsRedirection();

        return app;
    }
}