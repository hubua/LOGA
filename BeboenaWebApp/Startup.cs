using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BeboenaWebApp.Services;
using BeboenaWebApp.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Hosting;

namespace BeboenaWebApp
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
            //services.AddControllersWithViews();

            services.AddMvc(options => { options.Filters.Add(typeof(UserSettingsActionFilter)); });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
            });

            services.AddScoped<UserSettingsActionFilter>();

            services.AddHealthChecks();

            services.AddResponseCompression();

            /*
             * Brotli is HTTPS only https://samsaffron.com/archive/2016/06/15/the-current-state-of-brotli-compression
             * services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); //TODO IMPORTANT nice exception page
            }

            app.UseSession();

            app.UseHealthChecks("/healthcheck");

            app.UseResponseCompression(); // UseResponseCompression must be called before UseMvc

            app.UseStaticFiles(); // from wwwroot
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                RequestPath = "/lib",
                OnPrepareResponse = context =>
                {
                    var t = 60 * 60 * 24 * 365; // sec
                    //context.Context.Response.Headers.Append("Cache-Control", $"public,max-age={t}");

                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue { MaxAge = TimeSpan.FromSeconds(t) };
                }
            });

            app.UseStatusCodePages();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            var dirinfo = env.ContentRootFileProvider.GetFileInfo("/Services/Data");
            var dir = dirinfo.PhysicalPath;
            GeorgianABCService.Initialize(dir);
        }
    }

}
