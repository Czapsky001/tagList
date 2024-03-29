using Microsoft.EntityFrameworkCore;
using TagList.Convert;
using TagList.DatabaseConnector;
using TagList.Repositories.TagRepo;
using TagList.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TagList
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

            services.AddScoped<IConvertJson, ConvertJson>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagService, TagService>();

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var connectionString = DatabaseConfig.GetConnectionString();
            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(connectionString));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var serviceProvider = app.ApplicationServices;

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                var db = serviceProvider.GetRequiredService<DatabaseContext>().Database;

                logger.LogInformation("Migrating database...");

                while (!db.CanConnect())
                {
                    logger.LogInformation("Database not ready yet; waiting...");
                    Thread.Sleep(1000);
                }

                try
                {
                    db.Migrate();
                    logger.LogInformation("Database migrated successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            if (env.IsDevelopment())
            {


            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
