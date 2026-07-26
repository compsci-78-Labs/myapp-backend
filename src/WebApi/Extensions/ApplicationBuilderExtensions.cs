using WebApi.Features.Projects;
using WebApi.Features.TaskItems;
using WebApi.Features.Users;

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