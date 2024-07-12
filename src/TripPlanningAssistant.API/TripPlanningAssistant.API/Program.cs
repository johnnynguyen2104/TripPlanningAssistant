using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Supabase;
using TripPlanningAssistant.API.Options;
using TripPlanningAssistant.API.Services;

namespace TripPlanningAssistant.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.Configure<AWSBedrockConfigOptions>(builder.Configuration.GetSection(nameof(AWSBedrockConfigOptions)));
            builder.Services.Configure<SupabaseDbOptions>(builder.Configuration.GetSection(nameof(SupabaseDbOptions)));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient((provider) =>
            {
                var option = provider.GetService<IOptions<SupabaseDbOptions>>();
                return option.Value.SupabaseOptions is null 
                    ? new Client(option.Value.SupabaseUrl, option.Value.SupabaseKey) 
                    : new Client(option.Value.SupabaseUrl, option.Value.SupabaseKey, option.Value.SupabaseOptions);
            });

            builder.Services.AddTransient<AWSBedrockService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
