using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApi.Application.Auth;
using WebApi.Infrastructure;
using WebApi.Domain.Users;
using Program = WebApi.Program;

namespace Tests.Common;

public class WebApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection;

    public WebApiFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();

            services.AddDbContext<AppDbContext>(options => { options.UseSqlite(_connection); });

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
        });
    }

    public async Task<HttpClient> CreateAuthenticatedClientAsync(User user)
    {
        var client = CreateClient();

        using var scope = Services.CreateScope();
        var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();

        var accessToken = await identityService.GenerateAccessToken(user);
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        return client;
    }

    public async Task SeedDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await DbSeeder.Seed(db);
    }

    public async Task ClearDatabaseTablesAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Clear all data from the database
        dbContext.Users.RemoveRange(dbContext.Users);
        dbContext.Projects.RemoveRange(dbContext.Projects);
        dbContext.TaskItems.RemoveRange(dbContext.TaskItems);

        await dbContext.SaveChangesAsync();
    }

    public async Task ClearUsersTableAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Clear users table from the database
        dbContext.Users.RemoveRange(dbContext.Users);

        await dbContext.SaveChangesAsync();
    }

    public async Task ClearProjectsTableAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Clear projects table data from the database
        dbContext.Projects.RemoveRange(dbContext.Projects);

        await dbContext.SaveChangesAsync();
    }

    public async Task ClearTaskItemsTableAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Clear taskItmes table data from the database
        dbContext.TaskItems.RemoveRange(dbContext.TaskItems);

        await dbContext.SaveChangesAsync();
    }

    public async Task<AppDbContext> GetDbContextAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        return dbContext;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _connection.Dispose();
        }
    }
}