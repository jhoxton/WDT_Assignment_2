using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WDT_Assignment_2.Data;
using WDT_Assignment_2.Models;
using Microsoft.AspNetCore.Identity;
using WDT_Assignment_2.Services;

namespace WDT_Assignment_2
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
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddDbContext<Context>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Context")));
            
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
               
                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = options.Password.RequireNonAlphanumeric =
                options.Password.RequireUppercase = options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });

            services.AddMvc();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();



            app.UseMvc(routes =>
            {
                
                routes.MapRoute(
                    
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }

    }
}
