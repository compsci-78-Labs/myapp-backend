using WebApi.Presentation.Auth;
using WebApi.Presentation.Projects;
using WebApi.Presentation.TaskItems;
using WebApi.Presentation.Users;

namespace WebApi.Extensions;

public static  class ApplicationBuilderExtensions
{
    public static WebApplication MapApplicationEndpoints(
        this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapUserEndpoints();
        app.MapProjectEndpoints();
        app.MapTaskItemEndpoints();

        return app;
    }
}