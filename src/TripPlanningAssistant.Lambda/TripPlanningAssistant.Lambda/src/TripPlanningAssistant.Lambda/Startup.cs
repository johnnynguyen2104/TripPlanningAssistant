using Microsoft.Extensions.Options;
using Supabase;
using TripPlanningAssistant.API.Services;
using TripPlanningAssistant.Lambda.Options;

namespace TripPlanningAssistant.Lambda
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<AWSBedrockConfigOptions>(Configuration.GetSection(nameof(AWSBedrockConfigOptions)));
            services.Configure<SupabaseDbOptions>(Configuration.GetSection(nameof(SupabaseDbOptions)));

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddTransient<AWSBedrockService>();
            services.AddTransient((provider) =>
            {
                var option = provider.GetService<IOptions<SupabaseDbOptions>>();
                return option.Value.SupabaseOptions is null
                    ? new Client(option.Value.SupabaseUrl, option.Value.SupabaseKey)
                    : new Client(option.Value.SupabaseUrl, option.Value.SupabaseKey, option.Value.SupabaseOptions);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
                });
            });
        }
    }
}