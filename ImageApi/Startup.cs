using System.Text;
using ImageApi.Converters;
using ImageApi.Interfaces;
using ImageApi.Model;
using ImageApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;

namespace ImageApi
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
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "ImageApi",
                    Description = "Api for image conversion",
                    TermsOfService = "None",
                    Contact = new Contact()
                    {
                        Name = "Łukasz Brzezinski",
                        Email = "lukasz.grzegorz.brzezinski@gmail.com",
                        Url = "http://lukaszbrzezinski.pl/"
                    }
                });
            });

            services.AddSingleton<IImageService, ImageService>();
            services.AddSingleton<IImageServiceData, ImageServiceData>();
            services.AddSingleton<IFileSystemService, FileSystemService>();
            services.AddSingleton<IImageConverter, ImageConverter>();
            services.AddSingleton<IThumbnailConverter, ThumbnailConverter>();
            services.AddSingleton<ISettings, Settings.Settings>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            env.ConfigureNLog("nlog.config");
            //loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ImageAPI V1"); });
        }
    }
}
