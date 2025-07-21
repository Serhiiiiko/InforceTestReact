using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using InforceTestReact.Server.Data;

namespace InforceTestReact.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext CreateInMemoryDbContext()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<ApplicationDbContext>();
        }
    }
}