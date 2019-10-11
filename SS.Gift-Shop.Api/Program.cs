using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SS.GiftShop.Api.Identity;
using SS.GiftShop.Persistence;

namespace SS.GiftShop.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var identityInitializer = scope.ServiceProvider.GetRequiredService<IdentityInitializer>();
                // TODO: Get from configuration
                await identityInitializer.Run("admin@mail.com");

                var appInitializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();
                await appInitializer.Run();
            }

            await host.RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
