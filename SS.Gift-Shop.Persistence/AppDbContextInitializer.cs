using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SS.GiftShop.Domain.Entities;
using SS.GiftShop.Domain.Model;

namespace SS.GiftShop.Persistence
{
    public sealed class AppDbContextInitializer
    {
        private readonly AppDbContext _context;

        public AppDbContextInitializer(AppDbContext context)
        {
            _context = context;
        }

        public async Task Run()
        {
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.MigrateAsync();
            }

#if DEBUG
            if (!await _context.Examples.AnyAsync())
            {
                _context.Examples.AddRange(
                    new Example { Name = "Pasta", NormalizedName = "PASTA", Email = "pasta@food.com", Status = EnabledStatus.Enabled },
                    new Example { Name = "Tacos", NormalizedName = "TACOS", Email = "tacos@food.com", Status = EnabledStatus.Enabled },
                    new Example { Name = "Soup", NormalizedName = "SOUP", Email = "soup@food.com", Status = EnabledStatus.Enabled },
                    new Example { Name = "Pizza", NormalizedName = "PIZZA", Email = "pizza@food.com", Status = EnabledStatus.Enabled },
                    new Example { Name = "Burger", NormalizedName = "BURGER", Email = "burger@food.com", Status = EnabledStatus.Enabled },
                    new Example { Name = "Salad", NormalizedName = "SALAD", Email = "salad@food.com", Status = EnabledStatus.Enabled }
                );

                await _context.SaveChangesAsync();
            }
#endif
        }
    }
}
