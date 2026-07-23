using WebApi.Features.Project;
using WebApi.Features.Task;
using WebApi.Features.User;

namespace WebApi.Extensions;

public static  class ApplicationBuilderExtensions
{
    public static WebApplication MapApplicationEndpoints(
        this WebApplication app)
    {
        app.MapUserEndpoints();
        app.MapProjectEndpoints();
        app.MapTaskEndpoints();

        return app;
    }
}