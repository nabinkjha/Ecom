using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ECom.Contracts.Data.Repositories;
using ECom.Core.Data;
using ECom.Core.Data.Repositories;
using System.Linq;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.OData.Routing;
using System.Collections;
using Microsoft.AspNetCore.Http;
using ECom.API.Utilities;

namespace ECom.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>((builder) =>
            {
                builder.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection"),
                    (x) => x.MigrationsAssembly("ECom.Core"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddControllers().AddOData(opt =>
            opt.Select().Filter().Count().OrderBy().Expand().SetMaxTop(100)
            .AddRouteComponents("v1", EdmModelBuilder.Build("v1"))
            .AddRouteComponents("v2", EdmModelBuilder.Build("v2"))
            );
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddMvc(c => c.Conventions.Add(new GroupControllerByVersion()));// decorate Controllers to distinguish SwaggerDoc (v1, v2, etc.)
            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<CustomSwaggerFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ECom API",
                    Description = "ASP.NET Core Web API with OData",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Nabin Jha",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/nabinjha"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "API v2", Version = "v2" });
           
                //c.ExampleFilters();
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                // Uses full schema names to avoid v1/v2/v3 schema collisions
                // see: https://github.com/domaindrivendev/Swashbuckle/issues/442
                c.CustomSchemaIds(x => x.FullName);
            });
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            // a test middleware
            app.Use(next => context =>
            {
                var endpoint = context.GetEndpoint();
                if (endpoint == null)
                {
                    return next(context);
                }
                IEnumerable templates;
                IODataRoutingMetadata metadata = endpoint.Metadata.GetMetadata<IODataRoutingMetadata>();
                if (metadata != null)
                {
                    templates = metadata.Template.GetTemplates();
                }
                return next(context); 
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.RoutePrefix = string.Empty;
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
