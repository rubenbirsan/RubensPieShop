using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RubensPieShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace RubensPieShop
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>(); 

            //services.AddTransient<IPieRepository, MockPieRepository>();
            services.AddTransient<IPieRepository, PieRepository>();
            services.AddTransient<IFeedbackRepository, FeedbackRepository>();

            services.AddDirectoryBrowser();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles(); // For the wwwroot folder

            // For custom folder with static files
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //            Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
            //    RequestPath = "/StaticFiles"
            //});

            // For browsing images
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
                RequestPath = "/MyImages"
            });

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name:"default",
                    template:"{controller=Home}/{action=Index}/{id?}"                
                    );
            });
            //app.UseMvcWithDefaultRoute();

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".image"] = "image/jpg";
            provider.Mappings[".image"] = "image/png";

        }
    }
}
