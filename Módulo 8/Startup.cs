using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Módulo_8.Models;
using Módulo_8.Entities;
using Módulo_8.Helpers;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace Módulo_8
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
            // Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection -Version 7.0.0       | BOTH
            // Install-Package Microsoft.Extensions.DependencyInjection -Version 3.0.1                  | BOTH
            services.AddAutoMapper(options => options.CreateMap<AutorCreacionDTO, Autor>(), typeof(Startup));
            services.AddAutoMapper(options => options.CreateMap<Autor, AutorDTO>(), typeof(Startup));

            services
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped<IUrlHelper>(x => x
                .GetRequiredService<IUrlHelperFactory>()
                .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));

            services.AddScoped<HATEOASAuthorFilterAttribute>();
            services.AddScoped<HATEOASAuthorsFilterAttribute>();
            services.AddScoped<GeneradorEnlaces>();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info
                {
                    Title = "Web API",
                    Version = "v1",
                    Description = "Descripción del Web API",
                    TermsOfService = "https://es.wikipedia.org/wiki/Lorem_ipsum",
                    License = new License()
                    {
                        Name = "MIT",
                        Url = "https://es.wikipedia.org/wiki/Licencia_MIT"
                    },
                    Contact = new Contact()
                    {
                        Name = "Franco Vicentin",
                        Email = "fr.vicentin@gmail.com",
                        Url = "https://www.google.com.ar/?hl=es-419"
                    }
                });

                config.SwaggerDoc("v2", new Info
                {
                    Title = "Web API",
                    Version = "v2",
                    Description = "Descripción del Web API",
                    TermsOfService = "https://es.wikipedia.org/wiki/Lorem_ipsum",
                    License = new License()
                    {
                        Name = "MIT",
                        Url = "https://es.wikipedia.org/wiki/Licencia_MIT"
                    },
                    Contact = new Contact()
                    {
                        Name = "Franco Vicentin",
                        Email = "fr.vicentin@gmail.com",
                        Url = "https://www.google.com.ar/?hl=es-419"
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);

            });

            services.AddControllers();            

            services.AddMvc(config =>
            {
                config.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API V1");
                config.SwaggerEndpoint("/swagger/v2/swagger.json", "Web API V2");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
        {
            public void Apply(ControllerModel controller)
            {
                // Ejemplo: "Controllers.V1"
                var controllerNamespace = controller.ControllerType.Namespace;
                var apiVersion = controllerNamespace.Split('.').Last().ToLower();
                controller.ApiExplorer.GroupName = apiVersion;
            }
        }
    }
}
