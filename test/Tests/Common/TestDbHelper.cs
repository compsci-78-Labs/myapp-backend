using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Data;

namespace Tests.Common;

public static class TestDbHelper
{
    public static async Task<TEntity?> GetEntityAsync<TEntity>(
        IServiceProvider services, 
        Expression<Func<TEntity, bool>> predicate
    ) where TEntity : class
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    public static async Task<TResult?> GetEntityPropertyAsync<TEntity, TResult>(
        IServiceProvider services,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector
    ) where TEntity : class
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Apply the predicate and project using the selector
        return await dbContext.Set<TEntity>().Where(predicate).Select(selector).FirstOrDefaultAsync();
    }
    
}
