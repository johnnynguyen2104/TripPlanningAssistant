using Amazon.BedrockAgent;
using TripPlanningAssistant.API.Services;
using TripPlanningAssistant.Common.Options;

namespace TripPlanningAssistant.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.Configure<AWSBedrockConfigOptions>(builder.Configuration.GetSection(nameof(AWSBedrockConfigOptions)));
            builder.Services.Configure<SupabaseDbOptions>(builder.Configuration.GetSection(nameof(SupabaseDbOptions)));

            builder.Services.AddSingleton<AWSBedrockService>();

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
                pattern: "{controller=Agent}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
