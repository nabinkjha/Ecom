using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ECom.Contracts.Data.Repositories;
using ECom.Core.Data;
using ECom.Core.Entities;
using System;
using System.Linq;

namespace ECom.API
{
    public class Program
    {
#pragma warning disable CS1591
        public static void Main(string[] args)
        {

            var builder = CreateHostBuilder(args).Build();

            using (var sp = builder.Services.CreateScope())
            {
                var db = sp.ServiceProvider.GetRequiredService<DatabaseContext>();

                if (db.Database.GetPendingMigrations().Count() > 0)
                {
                    db.Database.Migrate();
                }

                var unitOfWork = sp.ServiceProvider.GetRequiredService<IUnitOfWork>();
             
                if (unitOfWork.Product.Count() == 0)
                {
                    Random rand = new Random();
                    for (int i = 1; i <= 10; i++)
                    {
                       
                        unitOfWork.ProductCategory.Add(new ProductCategory
                        {
                            Name = $"Product #{10 + i}",
                            Description = $"Popular product #{10 + i}",
                        });
                    }
                    for (int i = 1; i <= 1000; i++)
                    {
                        int number = rand.Next(1, 10);
                        unitOfWork.Product.Add(new Product
                        {
                            Name = $"Product #{1000 + i}",
                            Description = $"Popular product #{1000 + i}",
                            ProductCategoryId =number
                        });
                    }
                    unitOfWork.Commit();
                }
            }

            builder.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
#pragma warning restore CS1591
}
