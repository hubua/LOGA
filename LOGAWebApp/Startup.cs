using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LOGAWebApp.Services;
using LOGAWebApp.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Microsoft.Net.Http.Headers;

namespace LOGAWebApp
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

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); //TODO not working
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{lid?}");
            });

            var dirinfo = env.ContentRootFileProvider.GetFileInfo("/Services/Data");
            var dir = dirinfo.PhysicalPath;
            GeorgianABCService.Initialize(dir); //TODO inject svc
        }
    }

    public class BrotliCompressionProvider : ICompressionProvider
    {
        // https://blogs.msdn.microsoft.com/dotnet/2017/07/27/introducing-support-for-brotli-compression/
        // https://dotnet.myget.org/feed/dotnet-corefxlab/package/nuget/System.IO.Compression.Brotli

        public string EncodingName => "br";

        public bool SupportsFlush => true;

        public Stream CreateStream(Stream outputStream)
        {
            return new BrotliStream(outputStream, CompressionMode.Compress);
        }
    }
}
