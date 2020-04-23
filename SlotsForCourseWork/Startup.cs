using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using SlotsForCourseWork.Models;
using SlotsForCourseWork.Services;
using SlotsForCourseWork.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SlotsForCourseWork.ViewModels;
using Microsoft.AspNetCore.Mvc;
using BedeSlots.Services.Data;

namespace SlotsForCourseWork
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITransactionService, TransactionService>();

            // Use SQL Database if in Azure, otherwise, use SQLite
            services.AddDbContext<ApplicationContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection")));

            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequireNonAlphanumeric = false;
                opts.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationContext>();

            services.AddControllersWithViews();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();    // подключение аутентификации
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                            pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        

    }
}