using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SS.GiftShop.Domain.Entities;
using SS.GiftShop.Domain.Model;

namespace SS.GiftShop.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Example> Examples { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // To add new migrations, open a Command Prompt on the project directory and run:
            // dotnet ef migrations add <MigrationName> --context AppDbContext --startup-project ../SS.GiftShop.Api --output-dir Migrations

            // To apply migrations
            // dotnet ef database update --context AppDbContext --startup-project ../SS.GiftShop.Api

            // To revert migrations
            // dotnet ef migrations script <FromMigration> <ToMigration> --context AppDbContext --startup-project ../SS.GiftShop.Api --output revert.sql
            // Note: Use 0 to refer to the state before any migrations
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var type = entity.ClrType;

                if (type.GetInterfaces().Any(i => IsClosedTypeOf(i, typeof(IStatus<>))))
                {
                    modelBuilder.Entity(type).HasIndex(nameof(IStatus<string>.Status));
                }

                // Remove pluralization
                if (!typeof(ValueObject).IsAssignableFrom(type))
                {
                    entity.Relational().TableName = type.Name;
                }

                //foreach (var foreignKey in entity.GetForeignKeys())
                //{
                //    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                //}
            }
        }

        private static bool IsClosedTypeOf(Type type, Type genericInterfaceType)
        {
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == genericInterfaceType;
        }
    }
}
