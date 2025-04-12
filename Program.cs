using Microsoft.EntityFrameworkCore;
using MvcAppAws_Daniel_delaCruz.Services;
using Amazon.SecretsManager;
using MvcAppAws_Daniel_delaCruz.Context;
using MvcAppAws_Daniel_delaCruz.Models;

namespace MvcAppAws_Daniel_delaCruz
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

          
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //GESTION DEL SECRETO
            builder.Services.AddSingleton<SecretManagerService>();

            var config = builder.Configuration;

            var secretService = new SecretManagerService(config);
            string secretName = "";
            string connectionString = await secretService.GetSecretValuesAsync(secretName);


            //añadir dbContext y sqlserver
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization(); 

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
