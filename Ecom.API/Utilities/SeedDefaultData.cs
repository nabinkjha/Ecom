using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECom.Contracts.Data.Repositories;
using ECom.Core.Data;
using ECom.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ECom.API.Data
{
    public static class SeedDefaultData
    {
        public static void Seed(IHost builder)
        {
            using (var sp = builder.Services.CreateScope())
            {
                var db = sp.ServiceProvider.GetRequiredService<DatabaseContext>();

                if (db.Database.GetPendingMigrations().Count() > 0)
                {
                    db.Database.Migrate();
                }
                var unitOfWork = sp.ServiceProvider.GetRequiredService<IUnitOfWork>();
                Random rand = new Random();
                if (unitOfWork.ProductCategory.Count() == 0)
                {
                    for (int i = 1; i <= 500; i++)
                    {
                        unitOfWork.ProductCategory.Add(new ProductCategory
                        {
                            Name = $"Product Category_Name{10 + i}",
                            Description = $"Product Category_Description_{10 + i}",
                        });
                    }
                }
                if (unitOfWork.Product.Count() == 0)
                {
                    for (int i = 1; i <= 1000; i++)
                    {
                        int number = rand.Next(1, 10);
                        unitOfWork.Product.Add(new Product
                        {
                            Name = $"Product {1000 + i}",
                            Description = $"Popular product {1000 + i}",
                            SKU = "prod-" + i + "-" + number,
                            Slug = "popular-prod-" + i + "-" + number,
                            ImageUrl = $"/images/{i}_{number}.png",
                            ProductCategoryId = number
                        });
                    }
                }
                if (unitOfWork.ApiClient.Count() == 0)
                {
                    unitOfWork.ApiClient.Add(new ApiClient
                    {
                        Name = $"ecom-web",
                        Key = $"12345abecdf516d0f6472d5199999",
                        IsBlocked =false
                    });
                }
                unitOfWork.Commit();
            }
        }
    }
}
