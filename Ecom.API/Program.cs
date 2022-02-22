using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ECom.Contracts.Data.Repositories;
using ECom.Core.Data;
using ECom.Core.Entities;
using System;
using System.Linq;
using ECom.API.Data;

namespace ECom.API
{
    public class Program
    {
#pragma warning disable CS1591
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();
            SeedDefaultData.Seed(builder);
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
