using CustomerManagementSystem.Data;
using CustomerManagementSystem.DBContexts;
using CustomerManagementSystem.Repositories;
using CustomerManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddDbContext<CMSContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //builder.Services.AddDbContext<CMSContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            //                         sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

            // Repositories
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // CMS Services
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddHostedService<SynchronizationService>();

            builder.Services.AddControllers();

            ////  ???  Add database connection string
            //builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // Database Initialization
            CreateDbIfNotExists(app);

            // Generate DB Initialization Script
            GenerateDatabaseInitializationScript(app);

            app.Run();
        }

        private static void CreateDbIfNotExists(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<CMSContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        private static void GenerateDatabaseInitializationScript(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<CMSContext>();

                    // Generate SQL script for the initial database creation
                    var initialDatabaseScript = context.Database.GenerateCreateScript();

                    // Get the application's root directory
                    string appRootDirectory = AppContext.BaseDirectory;

                    // Specify the export file path relative to the root directory
                    string exportFilePath = Path.Combine(appRootDirectory, "InitialDatabaseScript.sql");

                    // Save the initial database script to the export file
                    File.WriteAllText(exportFilePath, initialDatabaseScript);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError($"{ex.Message}");
                }
            }
        }
    }
}