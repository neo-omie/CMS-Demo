using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CMS.Persistence.AutoService
{
    public class AutoUpdateService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public AutoUpdateService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<CMSDbContext>();
                        await ExecuteStoredProceduresAsync(dbContext, stoppingToken);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        private async Task ExecuteStoredProceduresAsync(CMSDbContext dbContext, CancellationToken cancellationToken)
        {
            await dbContext.Database.ExecuteSqlRawAsync("EXEC SP_AutoExpireContract", cancellationToken);
            await dbContext.Database.ExecuteSqlRawAsync("EXEC SP_AutoExpireClassifiedContract", cancellationToken);
        }
    }
}
